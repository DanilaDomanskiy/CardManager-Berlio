import { Routes, Route } from "react-router-dom";
import CardRecordsPage from "./Pages/CardRecordsPage";
import LoginPage from "./Pages/LoginPage";
import RegisterPage from "./Pages/RegisterPage";
import AdminRoute from "./AdminRoute";

function App() {
  return (
    <Routes>
      <Route path="/" element={<CardRecordsPage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="*" element={<CardRecordsPage />} />
      <Route element={<AdminRoute />}>
        <Route path="/register" element={<RegisterPage />} />
      </Route>
    </Routes>
  );
}

export default App;
