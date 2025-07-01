import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthContext";

const AdminRoute = ({ redirectPath = "/login", children }) => {
  const { isAdmin, loading } = useAuth();

  if (loading) {
    return <div>Загрузка...</div>;
  }

  if (!isAdmin) {
    return <Navigate to={redirectPath} replace />;
  }

  return children ? children : <Outlet />;
};

export default AdminRoute;
