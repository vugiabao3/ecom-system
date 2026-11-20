import React, { createContext, useContext, useState, useEffect, type ReactNode } from "react";
import { decodeToken, getToken, getRefreshToken, setAuthTokens, clearAuth, type DecodedUser } from "../utils/token";
import { logout as apiLogout } from "../services/authApi";
import { isDemoMode } from "../demo/store";
import { demoStore, type DemoSession } from "../demo/store";

interface AuthContextType {
    user: DecodedUser | null;
    token: string | null;
    isAuthenticated: boolean;
    isAdmin: boolean;
    isSeller: boolean;
    isShipper: boolean;
    login: (accessToken: string, refreshToken?: string | null) => void;
    demoLogin: (email: string, password: string) => boolean;
    logout: () => Promise<void>;
    refreshAuthUser: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [token, setToken] = useState<string | null>(() => isDemoMode() ? (demoStore.loadSession() ? "demo-token" : null) : getToken());
    const [user, setUser] = useState<DecodedUser | null>(() => {
        if (isDemoMode()) {
            const s = demoStore.loadSession();
            return s ? { id: s.userId, email: s.email, role: s.role } : null;
        }
        return decodeToken(getToken());
    });

    const refreshAuthUser = () => {
        if (isDemoMode()) {
            const s = demoStore.loadSession();
            setToken(s ? "demo-token" : null);
            setUser(s ? { id: s.userId, email: s.email, role: s.role } : null);
            return;
        }
        const currentToken = getToken();
        setToken(currentToken);
        setUser(decodeToken(currentToken));
    };

    useEffect(() => {
        refreshAuthUser();
    }, []);

    const login = (accessToken: string, refreshToken?: string | null) => {
        if (isDemoMode()) return;
        setAuthTokens(accessToken, refreshToken);
        setToken(accessToken);
        setUser(decodeToken(accessToken));
    };

    const demoLogin = (email: string, password: string): boolean => {
        if (!isDemoMode()) return false;
        const store = demoStore.load();
        const found = store.users.find(u => u.email === email && u.password === password);
        if (!found) return false;
        const session: DemoSession = { userId: found.id, email: found.email, fullName: found.fullName, role: found.role };
        demoStore.saveSession(session);
        setToken("demo-token");
        setUser({ id: found.id, email: found.email, role: found.role });
        return true;
    };

    const logout = async () => {
        if (isDemoMode()) {
            demoStore.saveSession(null);
            setToken(null);
            setUser(null);
            return;
        }
        const rToken = getRefreshToken();
        try {
            if (rToken) {
                await apiLogout({ refreshToken: rToken });
            }
        } catch {
            // Ignore failure on server logout
        } finally {
            clearAuth();
            setToken(null);
            setUser(null);
        }
    };

    const isAuthenticated = isDemoMode() ? !!demoStore.loadSession() : (!!token && !!user);
    const isAdmin = user?.role?.toLowerCase() === "admin";
    const isSeller = user?.role?.toLowerCase() === "seller";
    const isShipper = user?.role?.toLowerCase() === "shipper";

    return (
        <AuthContext.Provider
            value={{
                user,
                token,
                isAuthenticated,
                isAdmin,
                isSeller,
                isShipper,
                login,
                demoLogin,
                logout,
                refreshAuthUser,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};
