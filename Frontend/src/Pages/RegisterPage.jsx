import { useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import API_URL from "../host";

const RegisterPage = () => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isAdmin, setIsAdmin] = useState(false);

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const validate = () => {
    if (name.length < 2 || name.length > 30) {
      setError("Имя должно содержать от 2 до 30 символов.");
      return false;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setError("Введите корректный email.");
      return false;
    }

    const passwordRegex = /^(?=.*[A-Za-z]).{10,}$/;
    if (!passwordRegex.test(password)) {
      setError(
        "Пароль должен содержать минимум 10 символов и хотя бы одну латинскую букву."
      );
      return false;
    }

    return true;
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    if (!validate()) return;

    try {
      await axios.post(
        `${API_URL}/api/users`,
        {
          name,
          email,
          password,
          isAdmin,
        },
        { withCredentials: true }
      );
      setError(`Пользователь ${name} успешно зарегистрирован.`);
    } catch (err) {
      if (err.response?.status === 409) {
        setError("Пользователь с таким email уже зарегистрирован.");
      } else {
        console.error(err);
        setError("Сервер не отвечает. Пожалуйста, попробуйте позже.");
      }
    }
  };

  return (
    <div style={styles.wrapper}>
      <form onSubmit={handleRegister} style={styles.form}>
        <h2 style={styles.title}>Регистрация</h2>

        <input
          type="text"
          placeholder="Имя"
          value={name}
          onChange={(e) => setName(e.target.value)}
          style={styles.input}
          required
        />

        <input
          type="email"
          placeholder="Электронная почта"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
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

        <label style={styles.checkboxLabel}>
          <input
            type="checkbox"
            checked={isAdmin}
            onChange={(e) => setIsAdmin(e.target.checked)}
            style={{ marginRight: "8px" }}
          />
          Сделать администратором
        </label>

        {error && <div style={styles.error}>{error}</div>}
        {success && <div style={styles.success}>{success}</div>}

        <button type="submit" style={styles.button}>
          Зарегистрировать
        </button>

        <Link to="/" style={styles.link}>
          На главную
        </Link>
      </form>
    </div>
  );
};

export default RegisterPage;

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
    width: 320,
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
  success: {
    color: "green",
    fontSize: 14,
    marginBottom: 10,
    textAlign: "center",
  },
  checkboxLabel: {
    marginBottom: 12,
    fontSize: 14,
    color: "#333",
    display: "flex",
    alignItems: "center",
  },
  link: {
    marginTop: 12,
    textAlign: "center",
    display: "block",
    color: "#007acc",
    textDecoration: "none",
    fontSize: 16,
  },
};
