import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import API_URL from "../host";
import { useAuth } from "../AuthContext";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { checkAuth } = useAuth();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      setError("");
      await axios.post(
        `${API_URL}/api/users/login`,
        { login: username, password },
        { withCredentials: true }
      );
      await checkAuth();
      navigate("/");
    } catch (err) {
      if (err.response?.status === 409) {
        setError("Неверный логин и(или) пароль.");
      } else {
        console.error(err);
        setError("Сервер не отвечает. Пожалуйста, попробуйте позже.");
      }
    }
  };

  return (
    <div style={styles.wrapper}>
      <form onSubmit={handleLogin} style={styles.form}>
        <h2 style={styles.title}>Вход</h2>

        <input
          type="text"
          placeholder="Логин"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          style={styles.input}
          required
        />

        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          style={styles.input}
          required
        />

        {error && <div style={styles.error}>{error}</div>}

        <button type="submit" style={styles.button}>
          Войти
        </button>
      </form>
    </div>
  );
};

export default LoginPage;

const styles = {
  wrapper: {
    backgroundColor: "#f0f8ff",
    height: "100vh",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
  },
  form: {
    backgroundColor: "#fff",
    padding: 24,
    border: "1px solid #ccc",
    borderRadius: 8,
    width: 300,
    display: "flex",
    flexDirection: "column",
  },
  title: {
    marginBottom: 20,
    textAlign: "center",
    color: "#007acc",
  },
  input: {
    padding: 8,
    marginBottom: 12,
    borderRadius: 4,
    border: "1px solid #999",
  },
  button: {
    padding: 10,
    backgroundColor: "#007acc",
    color: "#fff",
    border: "none",
    borderRadius: 4,
    cursor: "pointer",
  },
  error: {
    color: "red",
    fontSize: 14,
    marginBottom: 10,
    textAlign: "center",
  },
};
