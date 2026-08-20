export const getToken = () =>
    localStorage.getItem("token");

export const getRefreshToken = () =>
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
