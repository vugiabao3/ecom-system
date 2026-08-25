import React, { createContext, useContext, useState, useEffect, type ReactNode } from "react";
import { decodeToken, getToken, getRefreshToken, setAuthTokens, clearAuth, type DecodedUser } from "../utils/token";
import { logout as apiLogout } from "../services/authApi";

interface AuthContextType {
    user: DecodedUser | null;
    token: string | null;
    isAuthenticated: boolean;
    isAdmin: boolean;
    isSeller: boolean;
    isShipper: boolean;
    login: (accessToken: string, refreshToken?: string | null) => void;
    logout: () => Promise<void>;
    refreshAuthUser: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [token, setToken] = useState<string | null>(() => getToken());
    const [user, setUser] = useState<DecodedUser | null>(() => decodeToken(getToken()));

    const refreshAuthUser = () => {
        const currentToken = getToken();
        setToken(currentToken);
        setUser(decodeToken(currentToken));
    };

    useEffect(() => {
        refreshAuthUser();
    }, []);

    const login = (accessToken: string, refreshToken?: string | null) => {
        setAuthTokens(accessToken, refreshToken);
        setToken(accessToken);
        setUser(decodeToken(accessToken));
    };

    const logout = async () => {
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

    const isAuthenticated = !!token && !!user;
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
