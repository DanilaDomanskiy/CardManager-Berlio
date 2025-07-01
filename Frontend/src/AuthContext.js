import { createContext, useContext, useEffect, useState } from "react";
import axios from "axios";
import API_URL from "./host";
import { useNavigate } from "react-router-dom";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const navigate = useNavigate();
  const [isAdmin, setIsAdmin] = useState(null);
  const [loading, setLoading] = useState(true);

  const checkAuth = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/users/isAdmin`, {
        withCredentials: true,
      });
      setIsAdmin(response.data.isAdmin);
    } catch (err) {
      if (err.response?.status === 401) {
        navigate("/login");
      } else {
        console.error(err);
        alert("Сервер не отвечает. Пожалуйста, попробуйте позже.");
      }
      setIsAdmin(false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <AuthContext.Provider value={{ isAdmin, loading, checkAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
