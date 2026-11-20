import React, { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from "react";
import { decodeToken, getToken, getRefreshToken, setAuthTokens, clearAuth, type DecodedUser } from "../utils/token";
import { logout as apiLogout } from "../services/authApi";
import { isDemoMode } from "../demo/store";
import { DEMO_UPDATE_EVENT, demoStore, verifyPassword, type DemoSession } from "../demo/store";

interface AuthContextType {
    user: DecodedUser | null;
    token: string | null;
    isInitialized: boolean;
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

const DEMO_TOKEN = "demo-token";

function readDemoSession(): DemoSession | null {
    const session = demoStore.loadSession();
    if (!session) return null;

    const user = demoStore.load().users.find((u) => u.id === session.userId);
    if (!user || (user.status && user.status !== "Active")) {
        demoStore.clearSession();
        return null;
    }

    if (user.email !== session.email || user.role !== session.role) {
        const repaired: DemoSession = {
            userId: user.id,
            email: user.email,
            fullName: user.fullName,
            role: user.role,
        };
        demoStore.saveSession(repaired);
        return repaired;
    }

    return session;
}

function toDecodedUser(session: DemoSession | null): DecodedUser | null {
    if (!session) return null;
    return { id: session.userId, email: session.email, role: session.role };
}

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const demo = isDemoMode();

    const [token, setToken] = useState<string | null>(() => {
        if (demo) return readDemoSession() ? DEMO_TOKEN : null;
        return getToken();
    });
    const [user, setUser] = useState<DecodedUser | null>(() => {
        if (demo) return toDecodedUser(readDemoSession());
        return decodeToken(getToken());
    });
    const [isInitialized, setIsInitialized] = useState<boolean>(false);

    const refreshAuthUser = useCallback(() => {
        if (demo) {
            const session = readDemoSession();
            setToken(session ? DEMO_TOKEN : null);
            setUser(toDecodedUser(session));
            return;
        }
        const currentToken = getToken();
        setToken(currentToken);
        setUser(decodeToken(currentToken));
    }, [demo]);

    useEffect(() => {
        refreshAuthUser();
        setIsInitialized(true);
    }, [refreshAuthUser]);

    useEffect(() => {
        if (!demo) return;

        const handleUpdate = (event: Event) => {
            const scope = (event as CustomEvent<{ scope?: string }>).detail?.scope;
            if (!scope || scope === "session" || scope === "store") {
                refreshAuthUser();
            }
        };
        const handleStorage = (event: StorageEvent) => {
            if (!event.key || event.key.startsWith("ecom_demo_v1_")) {
                refreshAuthUser();
            }
        };

        window.addEventListener(DEMO_UPDATE_EVENT, handleUpdate);
        window.addEventListener("storage", handleStorage);
        return () => {
            window.removeEventListener(DEMO_UPDATE_EVENT, handleUpdate);
            window.removeEventListener("storage", handleStorage);
        };
    }, [demo, refreshAuthUser]);

    const login = (accessToken: string, refreshToken?: string | null) => {
        if (demo) return;
        setAuthTokens(accessToken, refreshToken);
        setToken(accessToken);
        setUser(decodeToken(accessToken));
    };

    const demoLogin = (email: string, password: string): boolean => {
        if (!demo) return false;
        const normalized = email.trim().toLowerCase();
        const found = demoStore.load().users.find((u) => u.email.toLowerCase() === normalized);
        if (!found) return false;
        if (found.status && found.status !== "Active") return false;
        if (!verifyPassword(password, found.passwordHash)) return false;

        const session: DemoSession = {
            userId: found.id,
            email: found.email,
            fullName: found.fullName,
            role: found.role,
        };
        demoStore.saveSession(session);
        setToken(DEMO_TOKEN);
        setUser({ id: found.id, email: found.email, role: found.role });
        return true;
    };

    const logout = async () => {
        if (demo) {
            demoStore.clearSession();
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

    const isAuthenticated = demo ? !!user : !!token && !!user;
    const isAdmin = user?.role?.toLowerCase() === "admin";
    const isSeller = user?.role?.toLowerCase() === "seller";
    const isShipper = user?.role?.toLowerCase() === "shipper";

    return (
        <AuthContext.Provider
            value={{
                user,
                token,
                isInitialized,
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
