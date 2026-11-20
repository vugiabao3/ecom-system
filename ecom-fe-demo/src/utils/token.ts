export interface DecodedUser {
    id: string;
    email: string;
    role: string;
    exp?: number;
}

export const getToken = (): string | null =>
    localStorage.getItem("token");

export const getRefreshToken = (): string | null =>
    localStorage.getItem("refreshToken");

export const setAuthTokens = (
    accessToken: string,
    refreshToken?: string | null
) => {
    localStorage.setItem("token", accessToken);

    if (refreshToken) {
        localStorage.setItem("refreshToken", refreshToken);
    }
};

export const removeToken = () =>
    clearAuth();

export const clearAuth = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
};

export const decodeToken = (token?: string | null): DecodedUser | null => {
    const raw = token || getToken();
    if (!raw) return null;

    try {
        const parts = raw.split(".");
        if (parts.length < 2) return null;

        const base64Url = parts[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split("")
                .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
                .join("")
        );

        const payload = JSON.parse(jsonPayload);

        const role =
            payload.role ||
            payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
            "User";

        const id =
            payload.sub ||
            payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] ||
            "";

        const email =
            payload.email ||
            payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] ||
            "";

        return {
            id,
            email,
            role,
            exp: payload.exp,
        };
    } catch {
        return null;
    }
};

export const isTokenExpired = (token?: string | null): boolean => {
    const decoded = decodeToken(token);
    if (!decoded || !decoded.exp) return true;
    return Date.now() >= decoded.exp * 1000;
};
