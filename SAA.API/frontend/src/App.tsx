import { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [userData, setUserData] = useState<any>(() => {
    const saved = localStorage.getItem('userData');
    return saved ? JSON.parse(saved) : null;
  });
  
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  
  const [resultado, setResultado] = useState<any>(null);
  const [reporte, setReporte] = useState<any[] | null>(null);
  const [filtroPrograma, setFiltroPrograma] = useState<string>('');

  useEffect(() => {
    if (token) {
      if (userData?.rol === 'Administrador') {
        fetchReporteTodos();
      } else {
        fetchResultado();
      }
    }
  }, [token, userData?.rol]);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ nombreUsuario: username, contrasena: password })
      });
      const data = await res.json();
      if (res.ok && data.exito) {
        localStorage.setItem('token', data.token);
        localStorage.setItem('userData', JSON.stringify(data.usuario));
        setToken(data.token);
        setUserData(data.usuario);
      } else {
        setError(data.mensaje || 'Error al iniciar sesión');
      }
    } catch (err) {
      setError('Error de red al conectar con el servidor.');
    } finally {
      setLoading(false);
    }
  };

  const fetchResultado = async () => {
    setLoading(true);
    try {
      const res = await fetch('/api/postulantes/mi-resultado', {
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (res.ok) {
        const data = await res.json();
        setResultado(data);
      } else if (res.status === 401 || res.status === 403) {
        handleLogout();
      } else {
        setError('No se pudo cargar el resultado. Intente más tarde.');
      }
    } catch (err) {
      setError('Error de conexión.');
    } finally {
      setLoading(false);
    }
  };

  const fetchReporteTodos = async () => {
    setLoading(true);
    try {
      const res = await fetch('/api/admision/reporte-todos', {
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (res.ok) {
        const data = await res.json();
        setReporte(data);
      } else if (res.status === 401 || res.status === 403) {
        handleLogout();
      } else {
        setError('No se pudo cargar el reporte general.');
      }
    } catch (err) {
      setError('Error de conexión.');
    } finally {
      setLoading(false);
    }
  };

  const procesarResultados = async () => {
    if (!confirm('¿Estás seguro de volver a procesar los resultados?')) return;
    setLoading(true);
    try {
      const res = await fetch('/api/admision/procesar', {
        method: 'POST',
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (res.ok) {
        alert('✅ Resultados procesados exitosamente.');
        fetchReporteTodos();
      } else {
        alert('❌ Hubo un error al procesar los resultados.');
      }
    } catch (err) {
      alert('Error de conexión.');
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userData');
    setToken(null);
    setResultado(null);
    setReporte(null);
    setUserData(null);
    setUsername('');
    setPassword('');
  };

  if (!token) {
    return (
      <div className="login-container">
        <div className="login-box glass-panel">
          <div className="brand">
            <div className="logo-placeholder"></div>
            <h1>Portal SAA</h1>
            <p>Ingresa tus credenciales</p>
          </div>
          
          <form onSubmit={handleLogin} className="login-form">
            <div className="input-group">
              <label>Usuario / DNI</label>
              <input 
                type="text" 
                value={username} 
                onChange={(e) => setUsername(e.target.value)} 
                required 
                placeholder="Ingresa tu usuario"
              />
            </div>
            <div className="input-group">
              <label>Contraseña</label>
              <input 
                type="password" 
                value={password} 
                onChange={(e) => setPassword(e.target.value)} 
                required 
                placeholder="Tu contraseña"
              />
            </div>
            
            {error && <div className="error-alert">{error}</div>}
            
            <button type="submit" className="primary-btn" disabled={loading}>
              {loading ? 'Ingresando...' : 'Ingresar'}
            </button>
          </form>
        </div>
        <div className="bg-shapes">
          <div className="shape shape-1"></div>
          <div className="shape shape-2"></div>
        </div>
      </div>
    );
  }

  const [vistaAdmin, setVistaAdmin] = useState<'Ingresantes' | 'No Ingresantes'>('Ingresantes');

  // Dashboard de ADMINISTRADOR
  if (userData?.rol === 'Administrador') {
    const listadoBase = reporte ? (vistaAdmin === 'Ingresantes' ? reporte.filter(r => r.estado === 'Ingresante') : reporte.filter(r => r.estado !== 'Ingresante')) : [];
    const programasUnicos = Array.from(new Set(listadoBase.map(r => r.programaAcademico))).sort();
    const listadoFinal = filtroPrograma ? listadoBase.filter(r => r.programaAcademico === filtroPrograma) : listadoBase;

    return (
      <div className="dashboard-container">
        <header className="glass-header">
          <div className="header-content">
            <h2>Portal de Administración</h2>
            <div className="user-info">
              <span>Admin: {userData.nombreCompleto}</span>
              <button onClick={handleLogout} className="logout-btn">Cerrar Sesión</button>
            </div>
          </div>
        </header>

        <main className="dashboard-content" style={{ maxWidth: '1000px' }}>
          <div className="result-card glass-panel fade-in-up" style={{ padding: '30px' }}>
            <div className="result-header" style={{ marginBottom: '20px' }}>
              <h3>Gestión de Admisión</h3>
              <button onClick={procesarResultados} className="primary-btn" style={{ width: 'auto', marginTop: 0 }} disabled={loading}>
                {loading ? 'Procesando...' : '⚙️ Procesar Motor de Admisión'}
              </button>
            </div>
            
            <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
              <button 
                onClick={() => setVistaAdmin('Ingresantes')} 
                className={`primary-btn ${vistaAdmin === 'Ingresantes' ? 'active' : ''}`}
                style={{ width: 'auto', margin: 0, opacity: vistaAdmin === 'Ingresantes' ? 1 : 0.6 }}
              >
                Ver Ingresantes
              </button>
              <button 
                onClick={() => setVistaAdmin('No Ingresantes')} 
                className={`primary-btn ${vistaAdmin === 'No Ingresantes' ? 'active' : ''}`}
                style={{ width: 'auto', margin: 0, opacity: vistaAdmin === 'No Ingresantes' ? 1 : 0.6, background: 'var(--danger)' }}
              >
                Ver No Ingresantes
              </button>
            </div>
            
            <h4 style={{ marginBottom: '15px', color: 'var(--text-muted)' }}>
              {vistaAdmin === 'Ingresantes' ? 'Lista de Ingresantes' : 'Lista de Postulantes No Ingresantes'}
            </h4>
            
            {loading && !reporte ? (
              <div className="loading-spinner">Cargando reporte...</div>
            ) : reporte && reporte.length > 0 ? (
              <>
                <div style={{ marginBottom: '15px', display: 'flex', alignItems: 'center', gap: '10px' }}>
                  <label style={{ color: 'var(--text-muted)' }}>Filtrar por Programa:</label>
                  <select 
                    value={filtroPrograma} 
                    onChange={(e) => setFiltroPrograma(e.target.value)}
                    style={{ padding: '8px', borderRadius: '4px', border: '1px solid var(--glass-border)', background: 'var(--glass-bg)', color: 'white' }}
                  >
                    <option value="">Todos los programas</option>
                    {programasUnicos.map(prog => (
                      <option key={prog as string} value={prog as string}>{prog as string}</option>
                    ))}
                  </select>
                </div>
                {listadoFinal.length > 0 ? (
                  <div className="table-responsive">
                    <table style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
                      <thead>
                        <tr style={{ borderBottom: '1px solid var(--glass-border)' }}>
                          <th style={{ padding: '10px' }}>Puesto</th>
                          <th style={{ padding: '10px' }}>Postulante</th>
                          <th style={{ padding: '10px' }}>Programa</th>
                          <th style={{ padding: '10px' }}>Puntaje</th>
                          {vistaAdmin === 'No Ingresantes' && <th style={{ padding: '10px' }}>Estado</th>}
                        </tr>
                      </thead>
                      <tbody>
                        {listadoFinal.map((ing, i) => (
                          <tr key={i} style={{ borderBottom: '1px solid rgba(255,255,255,0.05)' }}>
                            <td style={{ padding: '10px', fontWeight: 'bold' }}>#{ing.puesto}</td>
                            <td style={{ padding: '10px' }}>{ing.nombres} {ing.apellidos}</td>
                            <td style={{ padding: '10px' }}>{ing.programaAcademico}</td>
                            <td style={{ padding: '10px', color: 'var(--primary)' }}>{ing.puntaje}</td>
                            {vistaAdmin === 'No Ingresantes' && (
                              <td style={{ padding: '10px' }}>
                                <span className={`status-badge ${ing.estado.toLowerCase()}`} style={{ fontSize: '0.8rem', padding: '4px 8px' }}>
                                  {ing.estado}
                                </span>
                              </td>
                            )}
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                ) : (
                  <div className="error-alert">No hay registros para los filtros seleccionados.</div>
                )}
              </>
            ) : (
              <div className="error-alert">Aún no se ha procesado a los ingresantes.</div>
            )}
          </div>
        </main>
        
        <div className="bg-shapes">
            <div className="shape shape-1"></div>
            <div className="shape shape-2"></div>
        </div>
      </div>
    );
  }

  // Dashboard de POSTULANTE
  return (
    <div className="dashboard-container">
      <header className="glass-header">
        <div className="header-content">
          <h2>Portal de Admisión</h2>
          <div className="user-info">
            <span>Hola, {resultado?.nombres || userData?.nombreCompleto || 'Postulante'}</span>
            <button onClick={handleLogout} className="logout-btn">Cerrar Sesión</button>
          </div>
        </div>
      </header>

      <main className="dashboard-content">
        {loading && !resultado ? (
          <div className="loading-spinner">Cargando resultados...</div>
        ) : resultado ? (
          <div className="result-card glass-panel fade-in-up">
            <div className="result-header">
              <h3>Estado de Admisión</h3>
              <div className={`status-badge ${resultado.estado.toLowerCase()}`}>
                {resultado.estado}
              </div>
            </div>
            
            <div className="result-body">
              <div className="info-item">
                <span className="label">Programa Académico</span>
                <span className="value">{resultado.programa}</span>
              </div>
              <div className="info-item">
                <span className="label">Puntaje Obtenido</span>
                <span className="value highlight">{resultado.puntaje} pts</span>
              </div>
              <div className="info-item">
                <span className="label">Puesto (Orden de Mérito)</span>
                <span className="value">#{resultado.puesto}</span>
              </div>
            </div>

            <div className="result-footer">
              {resultado.estado === 'Ingresante' ? (
                <div className="success-message">
                  🎉 ¡Felicidades! Has logrado una vacante.
                </div>
              ) : resultado.estado === 'Aprobado' ? (
                <div className="warning-message">
                  Has aprobado el examen, pero no se alcanzó una vacante debido al límite de cupos del programa.
                </div>
              ) : (
                <div className="danger-message">
                  Lo sentimos, no has alcanzado el puntaje mínimo aprobatorio.
                </div>
              )}
            </div>
          </div>
        ) : (
          <div className="error-alert">No hay resultados disponibles para tu perfil.</div>
        )}
      </main>
      <div className="bg-shapes">
          <div className="shape shape-1"></div>
          <div className="shape shape-2"></div>
      </div>
    </div>
  );
}

export default App;
