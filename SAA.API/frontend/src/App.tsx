import { useState, useEffect, useCallback } from 'react';
import './App.css';

const API_BASE_URL = import.meta.env.PROD ? 'https://sistema-admision-wlii.onrender.com' : '';

type Vista = 'reporte' | 'estadisticas' | 'buscador';

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

  // Postulante
  const [resultado, setResultado] = useState<any>(null);
  const [miDetalle, setMiDetalle] = useState<any>(null);
  const [vistaPost, setVistaPost] = useState<'resumen' | 'detalle'>('resumen');

  // Admin
  const [reporte, setReporte] = useState<any[]>([]);
  const [filtroPrograma, setFiltroPrograma] = useState('');
  const [vistaAdmin, setVistaAdmin] = useState<Vista>('reporte');
  const [subVista, setSubVista] = useState<'ingresantes' | 'todos'>('ingresantes');
  const [estadisticas, setEstadisticas] = useState<any>(null);
  const [busquedaDni, setBusquedaDni] = useState('');
  const [resultadoBusqueda, setResultadoBusqueda] = useState<any>(null);
  const [loadingBusqueda, setLoadingBusqueda] = useState(false);
  const [errorBusqueda, setErrorBusqueda] = useState<string | null>(null);
  const [exportando, setExportando] = useState(false);

  const handleLogout = useCallback(() => {
    localStorage.removeItem('token');
    localStorage.removeItem('userData');
    setToken(null); setResultado(null); setReporte([]);
    setUserData(null); setMiDetalle(null); setEstadisticas(null);
    setUsername(''); setPassword('');
  }, []);

  const authHeader = useCallback(() => ({
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }), [token]);

  const fetchResultado = useCallback(async () => {
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/postulantes/mi-resultado`, { headers: authHeader() });
      if (res.ok) setResultado(await res.json());
      else if (res.status === 401) handleLogout();
    } catch { setError('Error de conexión.'); }
    finally { setLoading(false); }
  }, [authHeader, handleLogout]);

  const fetchMiDetalle = useCallback(async () => {
    if (miDetalle) return;
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/postulantes/mi-detalle`, { headers: authHeader() });
      if (res.ok) setMiDetalle(await res.json());
      else if (res.status === 401) handleLogout();
    } catch { setError('Error al cargar detalle.'); }
    finally { setLoading(false); }
  }, [authHeader, handleLogout, miDetalle]);

  const fetchReporte = useCallback(async () => {
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/reporte-todos`, { headers: authHeader() });
      if (res.ok) setReporte(await res.json());
      else if (res.status === 401) handleLogout();
    } catch { setError('Error al cargar reporte.'); }
    finally { setLoading(false); }
  }, [authHeader, handleLogout]);

  const fetchEstadisticas = useCallback(async () => {
    if (estadisticas) return;
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/estadisticas`, { headers: authHeader() });
      if (res.ok) setEstadisticas(await res.json());
    } catch { setError('Error al cargar estadísticas.'); }
    finally { setLoading(false); }
  }, [authHeader, estadisticas]);

  useEffect(() => {
    if (!token) return;
    if (userData?.rol === 'Administrador') fetchReporte();
    else { fetchResultado(); }
  }, [token]);

  useEffect(() => {
    if (vistaAdmin === 'estadisticas') fetchEstadisticas();
  }, [vistaAdmin]);

  useEffect(() => {
    if (vistaPost === 'detalle') fetchMiDetalle();
  }, [vistaPost]);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true); setError(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ nombreUsuario: username, contrasena: password })
      });
      const data = await res.json();
      if (res.ok && data.exito) {
        localStorage.setItem('token', data.token);
        localStorage.setItem('userData', JSON.stringify(data.usuario));
        setToken(data.token); setUserData(data.usuario);
      } else { setError(data.mensaje || 'Credenciales incorrectas.'); }
    } catch { setError('Error de red.'); }
    finally { setLoading(false); }
  };

  const procesarResultados = async () => {
    if (!confirm('¿Procesar resultados del motor de admisión?')) return;
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/procesar`, { method: 'POST', headers: authHeader() });
      if (res.ok) { alert('✅ Resultados procesados.'); setEstadisticas(null); fetchReporte(); }
      else alert('❌ Error al procesar.');
    } finally { setLoading(false); }
  };

  const buscarPostulante = async () => {
    if (!busquedaDni.trim()) return;
    setLoadingBusqueda(true); setErrorBusqueda(null); setResultadoBusqueda(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/buscar/${busquedaDni.trim()}`, { headers: authHeader() });
      if (res.ok) setResultadoBusqueda(await res.json());
      else if (res.status === 404) setErrorBusqueda(`No se encontró postulante con DNI: ${busquedaDni}`);
      else setErrorBusqueda('Error al buscar.');
    } catch { setErrorBusqueda('Error de conexión.'); }
    finally { setLoadingBusqueda(false); }
  };

  const exportarCsv = async () => {
    setExportando(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/exportar-csv`, { headers: authHeader() });
      if (res.ok) {
        const blob = await res.blob();
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `resultados_admision_${new Date().toISOString().slice(0,10)}.csv`;
        a.click(); URL.revokeObjectURL(url);
      } else alert('Error al exportar.');
    } finally { setExportando(false); }
  };

  // ──────────────── LOGIN ────────────────
  if (!token) {
    return (
      <div className="login-container">
        <div className="login-box glass-panel">
          <div className="brand">
            <div className="logo-placeholder"></div>
            <h1>Portal SAA</h1>
            <p>Sistema de Admisión Académica</p>
          </div>
          <form onSubmit={handleLogin} className="login-form">
            <div className="input-group">
              <label>Usuario / DNI</label>
              <input type="text" value={username} onChange={e => setUsername(e.target.value)} required placeholder="Ingresa tu usuario" />
            </div>
            <div className="input-group">
              <label>Contraseña</label>
              <input type="password" value={password} onChange={e => setPassword(e.target.value)} required placeholder="Tu contraseña" />
            </div>
            {error && <div className="error-alert">{error}</div>}
            <button type="submit" className="primary-btn" disabled={loading}>
              {loading ? 'Ingresando...' : 'Ingresar'}
            </button>
          </form>
        </div>
        <div className="bg-shapes"><div className="shape shape-1"></div><div className="shape shape-2"></div></div>
      </div>
    );
  }

  // ──────────────── ADMIN ────────────────
  if (userData?.rol === 'Administrador') {
    const listadoBase = vistaAdmin === 'reporte'
      ? (subVista === 'ingresantes' ? reporte.filter(r => r.estado === 'Ingresante') : reporte)
      : [];
    const programasUnicos = [...new Set(reporte.map(r => r.programaAcademico))].sort();
    const listadoFinal = filtroPrograma ? listadoBase.filter(r => r.programaAcademico === filtroPrograma) : listadoBase;

    return (
      <div className="dashboard-container">
        <header className="glass-header">
          <div className="header-content">
            <div className="header-left">
              <span className="header-badge">⚙️ Admin</span>
              <h2>Portal de Administración — SAA</h2>
            </div>
            <div className="user-info">
              <span>👤 {userData.nombreCompleto}</span>
              <button onClick={handleLogout} className="logout-btn">Cerrar Sesión</button>
            </div>
          </div>
          {/* Nav tabs */}
          <nav className="admin-nav">
            <button className={`nav-tab ${vistaAdmin === 'reporte' ? 'active' : ''}`} onClick={() => setVistaAdmin('reporte')}>📋 Reporte General</button>
            <button className={`nav-tab ${vistaAdmin === 'buscador' ? 'active' : ''}`} onClick={() => setVistaAdmin('buscador')}>🔍 Buscar por DNI</button>
            <button className={`nav-tab ${vistaAdmin === 'estadisticas' ? 'active' : ''}`} onClick={() => setVistaAdmin('estadisticas')}>📊 Estadísticas</button>
          </nav>
        </header>

        <main className="dashboard-content" style={{ maxWidth: '1100px' }}>

          {/* ── REPORTE GENERAL ── */}
          {vistaAdmin === 'reporte' && (
            <div className="glass-panel fade-in-up" style={{ padding: '28px' }}>
              <div className="panel-header">
                <h3>Gestión de Admisión</h3>
                <div className="panel-actions">
                  <button onClick={procesarResultados} className="primary-btn btn-sm" disabled={loading}>
                    {loading ? '⏳' : '⚙️'} Procesar Motor
                  </button>
                  <button onClick={exportarCsv} className="export-btn btn-sm" disabled={exportando}>
                    {exportando ? '⏳ Exportando...' : '📥 Exportar CSV'}
                  </button>
                </div>
              </div>

              <div className="tab-group" style={{ marginBottom: '20px' }}>
                <button className={`tab-btn ${subVista === 'ingresantes' ? 'active' : ''}`} onClick={() => setSubVista('ingresantes')}>✅ Ingresantes</button>
                <button className={`tab-btn ${subVista === 'todos' ? 'active' : ''}`} onClick={() => setSubVista('todos')}>👥 Todos</button>
              </div>

              <div className="filter-row">
                <label>Filtrar por programa:</label>
                <select value={filtroPrograma} onChange={e => setFiltroPrograma(e.target.value)} className="select-filter">
                  <option value="">Todos los programas</option>
                  {programasUnicos.map(p => <option key={p as string} value={p as string}>{p as string}</option>)}
                </select>
                <span className="count-badge">{listadoFinal.length} registros</span>
              </div>

              {loading && !reporte.length ? (
                <div className="loading-spinner">Cargando reporte...</div>
              ) : listadoFinal.length > 0 ? (
                <div className="table-responsive">
                  <table className="data-table">
                    <thead>
                      <tr>
                        <th>#</th><th>DNI</th><th>Postulante</th><th>Programa</th>
                        <th>Correctas</th><th>Puntaje</th><th>Estado</th>
                      </tr>
                    </thead>
                    <tbody>
                      {listadoFinal.map((r: any, i: number) => (
                        <tr key={i} className={r.estado === 'Ingresante' ? 'row-ingresante' : r.estado === 'Aprobado' ? 'row-aprobado' : 'row-desaprobado'}>
                          <td className="td-rank">#{r.puesto}</td>
                          <td className="td-dni">{r.dNI || r.dni}</td>
                          <td>{r.nombres} {r.apellidos}</td>
                          <td>{r.programaAcademico}</td>
                          <td className="td-score">{Math.round(r.puntaje)}/100</td>
                          <td className="td-score" style={{ color: 'var(--primary)' }}>{r.puntaje}</td>
                          <td><span className={`status-badge ${r.estado?.toLowerCase()}`}>{r.estado}</span></td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              ) : (
                <div className="error-alert">No hay datos. Procesa el motor de admisión primero.</div>
              )}
            </div>
          )}

          {/* ── BUSCADOR ── */}
          {vistaAdmin === 'buscador' && (
            <div className="glass-panel fade-in-up" style={{ padding: '28px' }}>
              <h3>🔍 Buscar Postulante por DNI</h3>
              <p style={{ color: 'var(--text-muted)', marginBottom: '20px' }}>
                Ingresa el DNI del postulante para ver su reporte completo de 100 respuestas.
              </p>

              <div className="search-row">
                <input
                  type="text" value={busquedaDni}
                  onChange={e => setBusquedaDni(e.target.value)}
                  onKeyDown={e => e.key === 'Enter' && buscarPostulante()}
                  placeholder="Ej: 12345678" className="search-input"
                  maxLength={12}
                />
                <button onClick={buscarPostulante} className="primary-btn btn-sm" disabled={loadingBusqueda}>
                  {loadingBusqueda ? '⏳ Buscando...' : '🔍 Buscar'}
                </button>
              </div>

              {errorBusqueda && <div className="error-alert">{errorBusqueda}</div>}

              {resultadoBusqueda && (
                <DetallePostulanteView data={resultadoBusqueda} />
              )}
            </div>
          )}

          {/* ── ESTADÍSTICAS ── */}
          {vistaAdmin === 'estadisticas' && (
            <div className="fade-in-up">
              {loading && !estadisticas ? (
                <div className="loading-spinner">Cargando estadísticas...</div>
              ) : estadisticas ? (
                <EstadisticasView data={estadisticas} />
              ) : (
                <div className="error-alert">No hay estadísticas. Procesa el motor de admisión primero.</div>
              )}
            </div>
          )}
        </main>

        <div className="bg-shapes"><div className="shape shape-1"></div><div className="shape shape-2"></div></div>
      </div>
    );
  }

  // ──────────────── POSTULANTE ────────────────
  return (
    <div className="dashboard-container">
      <header className="glass-header">
        <div className="header-content">
          <h2>Portal de Admisión — SAA</h2>
          <div className="user-info">
            <span>Hola, {resultado?.nombres || userData?.nombreCompleto || 'Postulante'}</span>
            <button onClick={handleLogout} className="logout-btn">Cerrar Sesión</button>
          </div>
        </div>
        <nav className="admin-nav">
          <button className={`nav-tab ${vistaPost === 'resumen' ? 'active' : ''}`} onClick={() => setVistaPost('resumen')}>📋 Mi Resultado</button>
          <button className={`nav-tab ${vistaPost === 'detalle' ? 'active' : ''}`} onClick={() => setVistaPost('detalle')}>📝 Mis Respuestas</button>
        </nav>
      </header>

      <main className="dashboard-content">
        {loading && !resultado ? (
          <div className="loading-spinner">Cargando...</div>
        ) : vistaPost === 'resumen' ? (
          resultado ? (
            <div className="result-card glass-panel fade-in-up">
              <div className="result-header">
                <h3>Estado de Admisión</h3>
                <div className={`status-badge ${resultado.estado?.toLowerCase()}`}>{resultado.estado}</div>
              </div>
              {/* Recuadro de correctas/incorrectas */}
              <div className="score-summary">
                <div className="score-box correctas">
                  <span className="score-num">{Math.round(resultado.puntaje)}</span>
                  <span className="score-label">✅ Correctas</span>
                </div>
                <div className="score-box incorrectas">
                  <span className="score-num">{100 - Math.round(resultado.puntaje)}</span>
                  <span className="score-label">❌ Incorrectas</span>
                </div>
                <div className="score-box total">
                  <span className="score-num">100</span>
                  <span className="score-label">📝 Total Preguntas</span>
                </div>
              </div>
              <div className="result-body">
                <div className="info-item"><span className="label">Programa Académico</span><span className="value">{resultado.programa}</span></div>
                <div className="info-item"><span className="label">Puntaje</span><span className="value highlight">{resultado.puntaje} / 100 pts</span></div>
                <div className="info-item"><span className="label">Orden de Mérito</span><span className="value">#{resultado.puesto}</span></div>
              </div>
              <div className="result-footer">
                {resultado.estado === 'Ingresante' ? (
                  <div className="success-message">🎉 ¡Felicidades! Has logrado una vacante.</div>
                ) : resultado.estado === 'Aprobado' ? (
                  <div className="warning-message">✅ Aprobaste el examen, pero no se alcanzó una vacante disponible.</div>
                ) : (
                  <div className="danger-message">❌ No alcanzaste el puntaje mínimo de 50 correctas.</div>
                )}
              </div>
              <button className="primary-btn" style={{ marginTop: '16px' }} onClick={() => setVistaPost('detalle')}>
                📝 Ver mis 100 respuestas →
              </button>
            </div>
          ) : <div className="error-alert">No hay resultados disponibles aún.</div>
        ) : (
          // Vista detalle
          loading && !miDetalle ? (
            <div className="loading-spinner">Cargando tus respuestas...</div>
          ) : miDetalle ? (
            <DetallePostulanteView data={miDetalle} />
          ) : <div className="error-alert">No se encontraron respuestas registradas.</div>
        )}
      </main>

      <div className="bg-shapes"><div className="shape shape-1"></div><div className="shape shape-2"></div></div>
    </div>
  );
}

// ─────────────────────────────────────────────────────────
// COMPONENTE: Tabla de 100 respuestas
// ─────────────────────────────────────────────────────────
function DetallePostulanteView({ data }: { data: any }) {
  const [filtroArea, setFiltroArea] = useState('Todas');
  const [soloInco, setSoloInco] = useState(false);
  const areas = ['Todas', ...Array.from(new Set(data.respuestas?.map((r: any) => r.area) || []))];
  const respuestasFiltradas = (data.respuestas || []).filter((r: any) => {
    const areaOk = filtroArea === 'Todas' || r.area === filtroArea;
    const incoOk = !soloInco || !r.esCorrecta;
    return areaOk && incoOk;
  });

  return (
    <div className="detalle-container">
      <div className="detalle-header">
        <div>
          <h3>📋 {data.nombres} {data.apellidos}</h3>
          <p style={{ color: 'var(--text-muted)' }}>DNI: {data.dNI || data.dni} · {data.programaAcademico}</p>
        </div>
        <span className={`status-badge ${data.estado?.toLowerCase()}`}>{data.estado}</span>
      </div>

      {/* Recuadro de puntaje */}
      <div className="score-summary">
        <div className="score-box correctas">
          <span className="score-num">{data.totalCorrectas}</span>
          <span className="score-label">✅ Correctas</span>
        </div>
        <div className="score-box incorrectas">
          <span className="score-num">{data.totalIncorrectas}</span>
          <span className="score-label">❌ Incorrectas</span>
        </div>
        <div className="score-box total">
          <span className="score-num">{data.puntaje}</span>
          <span className="score-label">🏆 Puntaje</span>
        </div>
        <div className="score-box puesto">
          <span className="score-num">#{data.puesto || '—'}</span>
          <span className="score-label">📊 Puesto</span>
        </div>
      </div>

      {/* Filtros */}
      <div className="filter-row" style={{ marginBottom: '16px' }}>
        <label>Área:</label>
        <div className="tab-group">
          {(areas as string[]).map(a => (
            <button key={a} className={`tab-btn ${filtroArea === a ? 'active' : ''}`} onClick={() => setFiltroArea(a)}>{a}</button>
          ))}
        </div>
        <label style={{ marginLeft: '16px', display: 'flex', alignItems: 'center', gap: '6px', cursor: 'pointer' }}>
          <input type="checkbox" checked={soloInco} onChange={e => setSoloInco(e.target.checked)} />
          Solo incorrectas
        </label>
        <span className="count-badge">{respuestasFiltradas.length} preguntas</span>
      </div>

      {/* Tabla */}
      <div className="table-responsive">
        <table className="data-table respuestas-table">
          <thead>
            <tr>
              <th>N°</th><th>Área</th><th>Pregunta</th>
              <th>Tu Respuesta</th><th>Correcta</th><th>Resultado</th>
            </tr>
          </thead>
          <tbody>
            {respuestasFiltradas.map((r: any) => (
              <tr key={r.numeroPregunta} className={r.esCorrecta ? 'row-correcta' : 'row-incorrecta'}>
                <td className="td-num">{r.numeroPregunta}</td>
                <td><span className="area-badge">{r.area}</span></td>
                <td className="td-enunciado">{r.enunciado}</td>
                <td className={`td-resp ${r.esCorrecta ? 'resp-ok' : 'resp-err'}`}>
                  <strong>{r.respuestaSeleccionada}</strong>
                  <span className="resp-opcion">
                    {r.respuestaSeleccionada === 'A' ? r.opcionA
                      : r.respuestaSeleccionada === 'B' ? r.opcionB
                      : r.respuestaSeleccionada === 'C' ? r.opcionC
                      : r.respuestaSeleccionada === 'D' ? r.opcionD : '—'}
                  </span>
                </td>
                <td className="td-resp resp-ok">
                  <strong>{r.respuestaCorrecta}</strong>
                  <span className="resp-opcion">
                    {r.respuestaCorrecta === 'A' ? r.opcionA
                      : r.respuestaCorrecta === 'B' ? r.opcionB
                      : r.respuestaCorrecta === 'C' ? r.opcionC
                      : r.opcionD}
                  </span>
                </td>
                <td>{r.esCorrecta ? <span className="check-ok">✓</span> : <span className="check-err">✗</span>}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// ─────────────────────────────────────────────────────────
// COMPONENTE: Panel de estadísticas
// ─────────────────────────────────────────────────────────
function EstadisticasView({ data }: { data: any }) {
  const total = data.totalPostulantes || 1;
  return (
    <div>
      {/* Cards métricas */}
      <div className="stats-grid">
        <div className="stat-card glass-panel">
          <div className="stat-icon">👥</div>
          <div className="stat-value">{data.totalPostulantes}</div>
          <div className="stat-label">Total Postulantes</div>
        </div>
        <div className="stat-card glass-panel ingresante-card">
          <div className="stat-icon">🏆</div>
          <div className="stat-value">{data.totalIngresantes}</div>
          <div className="stat-label">Ingresantes</div>
          <div className="stat-sub">{Math.round(data.totalIngresantes / total * 100)}% del total</div>
        </div>
        <div className="stat-card glass-panel aprobado-card">
          <div className="stat-icon">✅</div>
          <div className="stat-value">{data.totalAprobados}</div>
          <div className="stat-label">Aprobados (sin vacante)</div>
        </div>
        <div className="stat-card glass-panel desaprobado-card">
          <div className="stat-icon">❌</div>
          <div className="stat-value">{data.totalDesaprobados}</div>
          <div className="stat-label">Desaprobados</div>
        </div>
        <div className="stat-card glass-panel">
          <div className="stat-icon">📊</div>
          <div className="stat-value">{data.promedioGeneral}</div>
          <div className="stat-label">Promedio General</div>
        </div>
        <div className="stat-card glass-panel">
          <div className="stat-icon">📝</div>
          <div className="stat-value">{data.promedioCorrectas}</div>
          <div className="stat-label">Prom. Respuestas Correctas</div>
        </div>
        <div className="stat-card glass-panel">
          <div className="stat-icon">🔺</div>
          <div className="stat-value">{data.puntajeMaximo}</div>
          <div className="stat-label">Puntaje Máximo</div>
        </div>
        <div className="stat-card glass-panel">
          <div className="stat-icon">🔻</div>
          <div className="stat-value">{data.puntajeMinimo}</div>
          <div className="stat-label">Puntaje Mínimo</div>
        </div>
      </div>

      <div className="stats-row">
        {/* Distribución de puntajes */}
        <div className="glass-panel" style={{ padding: '24px', flex: 1 }}>
          <h4 style={{ marginBottom: '16px' }}>📊 Distribución de Puntajes</h4>
          {(data.distribucionPuntaje || []).map((d: any) => {
            const pct = Math.round(d.cantidad / total * 100);
            return (
              <div key={d.rango} className="bar-row">
                <span className="bar-label">{d.rango}</span>
                <div className="bar-track">
                  <div className="bar-fill" style={{ width: `${pct}%` }}></div>
                </div>
                <span className="bar-count">{d.cantidad} ({pct}%)</span>
              </div>
            );
          })}
        </div>

        {/* Por programa */}
        <div className="glass-panel" style={{ padding: '24px', flex: 1 }}>
          <h4 style={{ marginBottom: '16px' }}>🎓 Por Programa Académico</h4>
          <table className="data-table" style={{ fontSize: '0.85rem' }}>
            <thead><tr><th>Programa</th><th>Postulantes</th><th>Ingresantes</th><th>Vacantes</th><th>Promedio</th></tr></thead>
            <tbody>
              {(data.porPrograma || []).map((p: any) => (
                <tr key={p.programa}>
                  <td>{p.programa}</td>
                  <td style={{ textAlign: 'center' }}>{p.totalPostulantes}</td>
                  <td style={{ textAlign: 'center', color: 'var(--success)' }}>{p.ingresantes}</td>
                  <td style={{ textAlign: 'center', color: 'var(--primary)' }}>{p.vacantes}</td>
                  <td style={{ textAlign: 'center' }}>{p.promedioPrograma}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Top 10 */}
      <div className="glass-panel" style={{ padding: '24px', marginTop: '20px' }}>
        <h4 style={{ marginBottom: '16px' }}>🏅 Top 10 Ingresantes</h4>
        <div className="table-responsive">
          <table className="data-table">
            <thead><tr><th>Puesto</th><th>DNI</th><th>Postulante</th><th>Programa</th><th>Puntaje</th></tr></thead>
            <tbody>
              {(data.top10 || []).map((r: any, i: number) => (
                <tr key={i} className="row-ingresante">
                  <td className="td-rank">🥇 #{r.puesto}</td>
                  <td className="td-dni">{r.dNI || r.dni}</td>
                  <td>{r.nombres} {r.apellidos}</td>
                  <td>{r.programaAcademico}</td>
                  <td className="td-score">{r.puntaje}/100</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default App;
