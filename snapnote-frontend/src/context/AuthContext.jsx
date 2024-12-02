import React, { createContext, useContext, useState } from 'react';

const AuthContext = createContext();

// provider component
export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Functions to handle login and logout
  const login = () => setIsAuthenticated(true);
  const logout = () => setIsAuthenticated(false);

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// custom hook to use the AuthContext
export const useAuth = () => {
  return useContext(AuthContext);
};
