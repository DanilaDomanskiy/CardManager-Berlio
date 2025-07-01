import { useEffect, useState, useRef, useCallback } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import API_URL from "../host";
import { useAuth } from "../AuthContext";

const CardRecordsPage = () => {
  const [records, setRecords] = useState([]);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [loading, setLoading] = useState(false);
  const [uploadLoading, setUploadLoading] = useState(false);

  const [cardNumber, setCardNumber] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [userName, setUserName] = useState("");
  const [filters, setFilters] = useState({});
  const [showFilter, setShowFilter] = useState(true);

  const [error, setError] = useState(null);
  const [uploadResults, setUploadResults] = useState(null);
  const [totalCount, setTotalCount] = useState(null);

  const observer = useRef();
  const navigate = useNavigate();
  const fileInputRef = useRef(null);

  const { isAdmin } = useAuth();

  const lastRowRef = useCallback(
    (node) => {
      if (loading) return;
      if (observer.current) observer.current.disconnect();
      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && hasMore) {
          setPage((prevPage) => prevPage + 1);
        }
      });
      if (node) observer.current.observe(node);
    },
    [loading, hasMore]
  );

  useEffect(() => {
    if (uploadResults) return;
    const fetchData = async () => {
      setLoading(true);
      setError(null);
      try {
        const res = await axios.get(`${API_URL}/api/card-records`, {
          params: {
            page,
            pageSize: 20,
            cardNumberStartsWith: filters.cardNumber || undefined,
            creatorNameStartsWith: filters.userName || undefined,
            startCreationDate: filters.startDate || undefined,
            endCreationDate: filters.endDate || undefined,
          },
          withCredentials: true,
        });
        setRecords((prev) =>
          page === 1 ? res.data.cardRecords : [...prev, ...res.data.cardRecords]
        );
        setHasMore(res.data.cardRecords.length === 20);
        setTotalCount(res.data.recordsCount);
      } catch (err) {
        if (err.response?.status === 401) {
          navigate("/login");
        } else {
          console.error(err);
          setError("Сервер не отвечает. Пожалуйста, попробуйте позже.");
        }
        setHasMore(false);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [page, filters, navigate, uploadResults]);

  const handleFilter = () => {
    setError(null);
    setPage(1);
    setHasMore(true);
    setRecords([]);
    setFilters({ cardNumber, startDate, endDate, userName });
  };

  const resetFilter = () => {
    setError(null);
    setCardNumber("");
    setStartDate("");
    setEndDate("");
    setUserName("");
    setFilters({});
    setPage(1);
    setRecords([]);
    setHasMore(true);
  };

  const handleFileUpload = async (event) => {
    const file = event.target.files[0];
    if (!file || file.type !== "text/csv") {
      alert("Пожалуйста, выберите CSV файл.");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);

    try {
      setUploadLoading(true);
      const res = await axios.post(`${API_URL}/api/card-records`, formData, {
        withCredentials: true,
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      setUploadResults(res.data);
      setTotalCount(res.data.length);
    } catch (error) {
      if (error.response?.status === 401) {
        navigate("/login");
      } else {
        console.error(error);
        setError("Ошибка загрузки файла.");
      }
    } finally {
      setUploadLoading(false);
      event.target.value = "";
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.topBar}>
        <h1 style={{ marginTop: 5, marginBottom: 5, color: "#007acc" }}>
          Berlio Cards
        </h1>
        <div style={{ display: "flex", gap: "10px", alignItems: "center" }}>
          {isAdmin && (
            <>
              <div style={styles.uploadContainer}>
                <input
                  type="file"
                  accept=".csv"
                  style={{ display: "none" }}
                  ref={fileInputRef}
                  onChange={handleFileUpload}
                />
                <button
                  onClick={() => fileInputRef.current.click()}
                  style={styles.uploadButton}
                >
                  Загрузить CSV
                </button>
              </div>
              <button
                onClick={() => navigate("/register")}
                style={styles.uploadButton}
              >
                Добавить пользователя
              </button>
            </>
          )}
          <button
            onClick={async () => {
              try {
                await axios.post(`${API_URL}/api/users/logout`, null, {
                  withCredentials: true,
                });
                navigate("/login");
              } catch (error) {
                console.error(error);
                navigate("/login");
              }
            }}
            style={styles.logoutLink}
          >
            Выйти
          </button>
        </div>
      </div>

      {uploadLoading && <div style={styles.loadingOverlay}>Загрузка...</div>}

      {!uploadResults && (
        <>
          <button
            onClick={() => setShowFilter((prev) => !prev)}
            style={styles.filterToggle}
          >
            {showFilter ? "Скрыть фильтры" : "Показать фильтры"}
          </button>

          {showFilter && (
            <div style={styles.filter}>
              <div style={styles.inputGroup}>
                <label htmlFor="cardNumber" style={styles.label}>
                  Номер карты
                </label>
                <input
                  id="cardNumber"
                  type="text"
                  value={cardNumber}
                  onChange={(e) => setCardNumber(e.target.value)}
                  style={styles.input}
                />
              </div>
              <div style={styles.inputGroup}>
                <label htmlFor="startDate" style={styles.label}>
                  Дата с
                </label>
                <input
                  id="startDate"
                  type="date"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                  style={styles.input}
                />
              </div>
              <div style={styles.inputGroup}>
                <label htmlFor="endDate" style={styles.label}>
                  Дата по
                </label>
                <input
                  id="endDate"
                  type="date"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                  style={styles.input}
                />
              </div>
              <div style={styles.inputGroup}>
                <label htmlFor="userName" style={styles.label}>
                  Имя пользователя
                </label>
                <input
                  id="userName"
                  name="userName"
                  type="text"
                  value={userName}
                  onChange={(e) => setUserName(e.target.value)}
                  style={styles.input}
                />
              </div>
              <button style={styles.button} onClick={handleFilter}>
                Фильтровать
              </button>
              <button style={styles.resetButton} onClick={resetFilter}>
                Сброс
              </button>
            </div>
          )}
        </>
      )}

      {error && (
        <div style={{ color: "red", marginBottom: "15px", fontWeight: "bold" }}>
          {error}
        </div>
      )}

      {totalCount !== null && (
        <div
          style={{ marginBottom: "10px", fontWeight: "bold", color: "#333" }}
        >
          Всего записей: {totalCount}
        </div>
      )}

      <div style={styles.tableContainer}>
        <table style={styles.table}>
          <thead>
            <tr>
              <th style={{ ...styles.th, width: "5%" }}>ID</th>
              <th style={{ ...styles.th, width: "11%" }}>Номер карты</th>
              <th style={{ ...styles.th, width: "22%" }}>Дорожка 1</th>
              <th style={{ ...styles.th, width: "11%" }}>Дорожка 2</th>
              <th style={{ ...styles.th, width: "30%" }}>Дорожка 3</th>
              <th style={{ ...styles.th, width: "11%" }}>Загружено</th>
              <th style={{ ...styles.th, width: "10%" }}>Дата создания</th>
            </tr>
          </thead>
          <tbody>
            {(uploadResults || records).map((r, idx) => (
              <tr
                key={r.id + "-" + idx}
                ref={
                  !uploadResults && idx === records.length - 1
                    ? lastRowRef
                    : null
                }
                style={
                  uploadResults
                    ? { backgroundColor: r.isAdded ? "#d4edda" : "#f8d7da" }
                    : {}
                }
              >
                <td style={styles.td}>{r.id || ""}</td>
                <td style={styles.td}>{r.cardNumber}</td>
                <td style={styles.td}>{r.track1}</td>
                <td style={styles.td}>{r.track2}</td>
                <td style={styles.td}>{r.track3}</td>
                <td style={{ ...styles.td, ...styles.tdCenter }}>
                  {uploadResults ? (r.isAdded ? "Да" : "Нет") : r.creator?.name}
                </td>
                <td style={{ ...styles.td, ...styles.tdCenter }}>
                  {r.created}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {loading && <p style={styles.loading}>Загрузка...</p>}
      </div>

      {uploadResults && (
        <button
          onClick={() => {
            setUploadResults(null);
            setPage(1);
            setRecords([]);
            setHasMore(true);
            setFilters((f) => ({ ...f }));
          }}
          style={{ ...styles.button, marginTop: "10px" }}
        >
          OK
        </button>
      )}
    </div>
  );
};

export default CardRecordsPage;

const styles = {
  container: {
    fontFamily: "Arial, sans-serif",
    padding: "20px",
    backgroundColor: "#f0f8ff",
    height: "100vh",
    boxSizing: "border-box",
    display: "flex",
    flexDirection: "column",
    overflow: "hidden",
    position: "relative",
  },
  topBar: {
    display: "flex",
    flexWrap: "wrap",
    justifyContent: "space-between",
    alignItems: "center",
    gap: "10px",
    marginBottom: "10px",
  },
  uploadContainer: {
    marginRight: "10px",
  },
  uploadButton: {
    backgroundColor: "#007acc",
    color: "#fff",
    padding: "6px 12px",
    fontSize: "14px",
    fontWeight: "bold",
    border: "none",
    cursor: "pointer",
  },
  logoutLink: {
    color: "#007acc",
    textDecoration: "none",
    fontSize: "18px",
    backgroundColor: "transparent",
    border: "none",
    cursor: "pointer",
  },
  filterToggle: {
    cursor: "pointer",
    backgroundColor: "#007acc",
    color: "#fff",
    padding: "6px 12px",
    fontSize: "14px",
    fontWeight: "bold",
    alignSelf: "flex-start",
    border: "none",
    marginBottom: "10px",
  },
  filter: {
    display: "flex",
    gap: "20px",
    padding: "15px",
    backgroundColor: "#e6f0fa",
    alignItems: "flex-end",
    marginBottom: "10px",
    flexWrap: "wrap",
  },
  inputGroup: {
    display: "flex",
    flexDirection: "column",
    fontSize: "14px",
    flex: "1 1 200px",
  },
  label: {
    marginBottom: "6px",
    fontWeight: "600",
    color: "#333",
  },
  input: {
    padding: "8px",
    border: "1px solid #ccc",
    borderRadius: 0,
    fontSize: "14px",
  },
  button: {
    padding: "10px",
    backgroundColor: "#007acc",
    color: "#fff",
    border: "none",
    borderRadius: 0,
    cursor: "pointer",
    height: "40px",
    flexShrink: 0,
    minWidth: "200px",
    fontWeight: "600",
    fontSize: "14px",
    alignSelf: "flex-end",
  },
  resetButton: {
    padding: "6px 10px",
    backgroundColor: "#ccc",
    color: "#000",
    border: "none",
    borderRadius: 0,
    cursor: "pointer",
    fontSize: "12px",
    height: "30px",
    alignSelf: "flex-end",
  },
  tableContainer: {
    flex: 1,
    overflowX: "auto",
    overflowY: "auto",
    border: "1px solid #ccc",
    backgroundColor: "#fff",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
    tableLayout: "fixed",
    minWidth: "1000px",
  },
  th: {
    backgroundColor: "#007acc",
    color: "white",
    padding: "10px",
    border: "1px solid #ccc",
    position: "sticky",
    top: 0,
    zIndex: 1,
    textAlign: "center",
    fontWeight: "600",
    fontSize: "14px",
    whiteSpace: "normal",
    wordWrap: "break-word",
  },
  td: {
    padding: "10px",
    border: "1px solid #ccc",
    wordBreak: "break-word",
    fontSize: "13px",
  },
  tdCenter: {
    textAlign: "center",
  },
  loading: {
    padding: "10px",
    textAlign: "center",
    fontStyle: "italic",
    color: "#555",
  },
  loadingOverlay: {
    position: "absolute",
    top: 0,
    left: 0,
    width: "100%",
    height: "100%",
    backgroundColor: "rgba(255, 255, 255, 0.7)",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    fontSize: "24px",
    fontWeight: "bold",
    color: "#007acc",
    zIndex: 10,
  },
};
