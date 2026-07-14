import { useState, useEffect, useCallback } from 'react';
import './App.css';

const API_BASE_URL = import.meta.env.PROD ? 'https://sistema-admision-wlii.onrender.com' : '';

type Vista = 'reporte' | 'estadisticas' | 'buscador' | 'programas' | 'carga-masiva';

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
  const [subVista, setSubVista] = useState<'ingresantes' | 'no_admitidos' | 'todos'>('ingresantes');
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

  const generarConstanciaPdf = () => {
    if (!resultado) return;
    const printWindow = window.open('', '_blank');
    if (!printWindow) return;

    const fecha = new Date().toLocaleDateString('es-PE', {
      day: 'numeric',
      month: 'long',
      year: 'numeric'
    });

    const htmlContent = `
      <!DOCTYPE html>
      <html>
      <head>
        <title>Constancia de Ingreso - SAA</title>
        <style>
          @import url('https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;800&family=Outfit:wght@300;400;500;600;700&display=swap');
          body {
            background-color: #ffffff;
            color: #1e293b;
            font-family: 'Outfit', sans-serif;
            margin: 0;
            padding: 40px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            box-sizing: border-box;
          }
          .certificate-border {
            border: 12px double #1e3a8a;
            padding: 30px;
            width: 100%;
            max-width: 800px;
            background: #fff;
            position: relative;
            box-shadow: 0 0 20px rgba(0,0,0,0.05);
            box-sizing: border-box;
          }
          .certificate-border::before {
            content: "";
            position: absolute;
            top: 5px; left: 5px; right: 5px; bottom: 5px;
            border: 2px solid #b45309;
            pointer-events: none;
          }
          .header {
            text-align: center;
            margin-bottom: 30px;
          }
          .university-crest {
            width: 80px;
            height: 80px;
            background: linear-gradient(135deg, #1e3a8a, #3b82f6);
            border-radius: 50%;
            margin: 0 auto 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: 700;
            font-size: 1.5rem;
            font-family: 'Cinzel', serif;
            box-shadow: 0 4px 10px rgba(30,58,138,0.2);
          }
          .university-name {
            font-family: 'Cinzel', serif;
            font-size: 1.25rem;
            font-weight: 700;
            color: #1e3a8a;
            margin: 0 0 4px;
            letter-spacing: 0.5px;
            text-transform: uppercase;
          }
          .office-name {
            font-size: 0.85rem;
            color: #64748b;
            font-weight: 600;
            letter-spacing: 1px;
            text-transform: uppercase;
            margin: 0;
          }
          .title {
            text-align: center;
            margin-bottom: 25px;
          }
          .title h1 {
            font-family: 'Cinzel', serif;
            font-size: 2.2rem;
            color: #b45309;
            margin: 0;
            font-weight: 800;
            letter-spacing: 1px;
          }
          .title p {
            color: #64748b;
            font-size: 0.95rem;
            margin: 6px 0 0;
            font-weight: 500;
          }
          .content {
            font-size: 1.05rem;
            line-height: 1.8;
            text-align: justify;
            margin-bottom: 40px;
            color: #334155;
          }
          .highlight {
            font-weight: 700;
            color: #0f172a;
          }
          .details-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 16px;
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            padding: 20px;
            margin: 24px 0;
          }
          .detail-item {
            display: flex;
            flex-direction: column;
            gap: 4px;
          }
          .detail-lbl {
            font-size: 0.78rem;
            color: #64748b;
            text-transform: uppercase;
            font-weight: 600;
            letter-spacing: 0.5px;
          }
          .detail-val {
            font-size: 1.05rem;
            font-weight: 700;
            color: #1e3a8a;
          }
          .signatures {
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            margin-top: 55px;
            padding: 0 10px;
          }
          .sig-box {
            text-align: center;
            width: 260px;
          }
          .sig-line {
            border-top: 1px solid #94a3b8;
            margin-bottom: 8px;
          }
          .sig-name {
            font-size: 0.88rem;
            font-weight: 700;
            color: #0f172a;
            display: block;
            margin-bottom: 2px;
          }
          .sig-title {
            font-size: 0.75rem;
            color: #64748b;
            font-weight: 600;
            line-height: 1.3;
          }
          .qr-placeholder {
            width: 90px;
            height: 90px;
            border: 1px solid #cbd5e1;
            padding: 4px;
            background: white;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 0.55rem;
            color: #94a3b8;
            text-align: center;
          }
          .footer-note {
            text-align: center;
            font-size: 0.75rem;
            color: #94a3b8;
            margin-top: 40px;
            border-top: 1px solid #f1f5f9;
            padding-top: 15px;
          }
          @media print {
            body { padding: 0; background: none; }
            .certificate-border { box-shadow: none; max-width: 100%; border-width: 8px; }
          }
        </style>
      </head>
      <body>
        <div class="certificate-border">
          <div class="header">
            <div class="university-crest">UNSCH</div>
            <h2 class="university-name">Universidad Nacional de San Cristóbal de Huamanga</h2>
            <p class="office-name">Oficina General de Admisión</p>
          </div>

          <div class="title">
            <h1>CONSTANCIA DE INGRESO</h1>
            <p>Proceso de Admisión Académica 2026</p>
          </div>

          <div class="content">
            La Oficina General de Admisión de la Universidad Nacional de San Cristóbal de Huamanga hace constar que el postulante
            <span class="highlight">${resultado.nombres} ${resultado.apellidos}</span>, identificado con DNI N° <span class="highlight">${userData?.nombreUsuario || ''}</span>,
            ha participado en el Examen General de Admisión y ha obtenido una vacante de ingreso en esta casa superior de estudios.
          </div>

          <div class="details-grid">
            <div class="detail-item">
              <span class="detail-lbl">Programa Académico</span>
              <span class="detail-val">${resultado.programa}</span>
            </div>
            <div class="detail-item">
              <span class="detail-lbl">Modalidad</span>
              <span class="detail-val">Examen General de Admisión</span>
            </div>
            <div class="detail-item">
              <span class="detail-lbl">Puntaje Obtenido</span>
              <span class="detail-val">${resultado.puntaje} / 100.00</span>
            </div>
            <div class="detail-item">
              <span class="detail-lbl">Orden de Mérito (Puesto)</span>
              <span class="detail-val">Puesto N° ${resultado.puesto}</span>
            </div>
          </div>

          <div class="signatures">
            <div class="sig-box">
              <div class="sig-line"></div>
              <span class="sig-name">Dr. Emilio Germán Ramírez Roca</span>
              <span class="sig-title">Rector de la UNSCH</span>
            </div>
            <div class="qr-placeholder">
              <img src="https://api.qrserver.com/v1/create-qr-code/?size=80x80&data=SAA-UNSCH-INGRESANTE-${userData?.nombreUsuario || ''}-${resultado.puesto}" alt="QR" width="80" height="80" />
            </div>
            <div class="sig-box">
              <div class="sig-line"></div>
              <span class="sig-name">Dr. Hermes Segundo Bermúdez Valqui</span>
              <span class="sig-title">Vicerrector Académico del VRAC UNSCH</span>
            </div>
          </div>

          <div class="footer-note">
            Ayacucho, ${fecha} · Documento Oficial con verificación electrónica por QR.
          </div>
        </div>
        <script>
          window.onload = function() {
            setTimeout(function() {
              window.print();
            }, 800);
          }
        </script>
      </body>
      </html>
    `;

    printWindow.document.write(htmlContent);
    printWindow.document.close();
  };

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
      ? (subVista === 'ingresantes' 
          ? reporte.filter(r => r.estado === 'Ingresante') 
          : subVista === 'no_admitidos' 
            ? reporte.filter(r => r.estado !== 'Ingresante') 
            : reporte)
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
            <button className={`nav-tab ${vistaAdmin === 'programas' ? 'active' : ''}`} onClick={() => setVistaAdmin('programas')}>🎓 Programas y Vacantes</button>
            <button className={`nav-tab ${vistaAdmin === 'carga-masiva' ? 'active' : ''}`} onClick={() => setVistaAdmin('carga-masiva')}>📥 Lectora Óptica (Carga)</button>
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
                <button className={`tab-btn ${subVista === 'no_admitidos' ? 'active' : ''}`} onClick={() => setSubVista('no_admitidos')}>❌ No Admitidos</button>
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
          {vistaAdmin === 'programas' && (
            <ProgramasView token={token} handleLogout={handleLogout} />
          )}

          {/* ── LECTORA ÓPTICA ── */}
          {vistaAdmin === 'carga-masiva' && (
            <CargaMasivaView token={token} />
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
                  <>
                    <div className="success-message" style={{ marginBottom: '12px' }}>🎉 ¡Felicidades! Has logrado una vacante.</div>
                    <button className="primary-btn" style={{ background: 'linear-gradient(135deg, #10b981, #059669)', marginTop: '4px', width: '100%' }} onClick={generarConstanciaPdf}>
                      📄 Descargar Constancia de Ingreso (PDF)
                    </button>
                  </>
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

// ─────────────────────────────────────────────────────────
// COMPONENTE: Gestión de Programas Académicos (CRUD)
// ─────────────────────────────────────────────────────────
interface ProgramasViewProps {
  token: string;
  handleLogout: () => void;
}

function ProgramasView({ token, handleLogout }: ProgramasViewProps) {
  const [programas, setProgramas] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Formulario nuevo / edición
  const [editId, setEditId] = useState<number | null>(null);
  const [codigo, setCodigo] = useState('');
  const [nombre, setNombre] = useState('');
  const [departamento, setDepartamento] = useState('');
  const [vacantes, setVacantes] = useState<number>(30);
  const [estado, setEstado] = useState('Activo');

  const headers = useCallback(() => ({
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }), [token]);

  const fetchProgramas = useCallback(async () => {
    setLoading(true); setError(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/programas`, { headers: headers() });
      if (res.ok) setProgramas(await res.json());
      else if (res.status === 401) handleLogout();
      else setError('Error al cargar programas.');
    } catch {
      setError('Error de conexión.');
    } finally {
      setLoading(false);
    }
  }, [headers, handleLogout]);

  useEffect(() => {
    fetchProgramas();
  }, [fetchProgramas]);

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!nombre.trim() || !codigo.trim()) {
      alert('Nombre y Código son requeridos.');
      return;
    }

    setLoading(true);
    try {
      const payload = {
        idProgramaAcademico: editId || 0,
        codigo: codigo.trim(),
        nombre: nombre.trim(),
        departamento: departamento.trim() || 'General',
        vacantes: vacantes,
        estado: estado
      };

      const res = await fetch(`${API_BASE_URL}/api/admision/programas`, {
        method: 'POST',
        headers: headers(),
        body: JSON.stringify(payload)
      });

      if (res.ok) {
        alert('✅ Programa académico guardado.');
        limpiarFormulario();
        fetchProgramas();
      } else {
        alert('❌ Error al guardar programa.');
      }
    } catch {
      alert('Error de conexión.');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (p: any) => {
    setEditId(p.idProgramaAcademico);
    setCodigo(p.codigo);
    setNombre(p.nombre);
    setDepartamento(p.departamento || '');
    setVacantes(p.vacantes || 0);
    setEstado(p.estado || 'Activo');
  };

  const handleDelete = async (id: number) => {
    if (!confirm('¿Estás seguro de eliminar este programa académico?')) return;
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/programas/${id}`, {
        method: 'DELETE',
        headers: headers()
      });
      if (res.ok) {
        alert('🗑️ Programa académico eliminado.');
        fetchProgramas();
      } else {
        alert('Error al eliminar.');
      }
    } catch {
      alert('Error de conexión.');
    } finally {
      setLoading(false);
    }
  };

  const limpiarFormulario = () => {
    setEditId(null);
    setCodigo('');
    setNombre('');
    setDepartamento('');
    setVacantes(30);
    setEstado('Activo');
  };

  return (
    <div className="glass-panel fade-in-up" style={{ padding: '28px' }}>
      <h3>🎓 Gestión de Programas Académicos y Vacantes</h3>
      <p style={{ color: 'var(--text-muted)', marginBottom: '20px' }}>
        Administra las carreras universitarias y el límite de cupos (vacantes) disponibles para el ingreso.
      </p>

      {/* Formulario */}
      <form onSubmit={handleSave} className="glass-panel" style={{ padding: '20px', marginBottom: '24px', background: 'rgba(255,255,255,0.02)' }}>
        <h4 style={{ marginBottom: '16px' }}>{editId ? '✏️ Editar Programa' : '➕ Añadir Nuevo Programa'}</h4>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))', gap: '12px', marginBottom: '14px' }}>
          <div className="input-group">
            <label>Código</label>
            <input type="text" value={codigo} onChange={e => setCodigo(e.target.value)} required placeholder="Ej: IS01" className="search-input" style={{ maxWidth: '100%' }} />
          </div>
          <div className="input-group">
            <label>Nombre de la Carrera</label>
            <input type="text" value={nombre} onChange={e => setNombre(e.target.value)} required placeholder="Ej: Ingeniería de Sistemas" className="search-input" style={{ maxWidth: '100%' }} />
          </div>
          <div className="input-group">
            <label>Facultad / Departamento</label>
            <input type="text" value={departamento} onChange={e => setDepartamento(e.target.value)} placeholder="Ej: Ingeniería" className="search-input" style={{ maxWidth: '100%' }} />
          </div>
          <div className="input-group">
            <label>Vacantes</label>
            <input type="number" value={vacantes} onChange={e => setVacantes(Number(e.target.value))} min={1} required className="search-input" style={{ maxWidth: '100%' }} />
          </div>
          <div className="input-group">
            <label>Estado</label>
            <select value={estado} onChange={e => setEstado(e.target.value)} className="select-filter" style={{ height: '45px', padding: '10px' }}>
              <option value="Activo">Activo</option>
              <option value="Inactivo">Inactivo</option>
            </select>
          </div>
        </div>
        <div style={{ display: 'flex', gap: '10px' }}>
          <button type="submit" className="primary-btn btn-sm" disabled={loading}>
            {editId ? 'Guardar Cambios' : 'Registrar Carrera'}
          </button>
          {editId && (
            <button type="button" onClick={limpiarFormulario} className="logout-btn" style={{ height: '40px' }}>
              Cancelar
            </button>
          )}
        </div>
      </form>

      {error && <div className="error-alert">{error}</div>}

      {/* Tabla */}
      {loading && !programas.length ? (
        <div className="loading-spinner">Cargando programas...</div>
      ) : (
        <div className="table-responsive">
          <table className="data-table">
            <thead>
              <tr>
                <th>Código</th><th>Programa / Carrera</th><th>Facultad</th>
                <th style={{ textAlign: 'center' }}>Vacantes</th><th>Estado</th><th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {programas.map((p: any) => (
                <tr key={p.idProgramaAcademico}>
                  <td style={{ fontFamily: 'monospace', fontWeight: 'bold' }}>{p.codigo}</td>
                  <td style={{ fontWeight: '600' }}>{p.nombre}</td>
                  <td>{p.departamento}</td>
                  <td style={{ textAlign: 'center', color: 'var(--primary)', fontWeight: 'bold' }}>{p.vacantes}</td>
                  <td>
                    <span className={`status-badge ${p.estado === 'Activo' ? 'ingresante' : 'pendiente'}`}>
                      {p.estado}
                    </span>
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '8px' }}>
                      <button onClick={() => handleEdit(p)} className="tab-btn" style={{ padding: '4px 10px', fontSize: '0.75rem' }}>✏️</button>
                      <button onClick={() => handleDelete(p.idProgramaAcademico)} className="logout-btn" style={{ padding: '4px 10px', fontSize: '0.75rem', border: '1px solid rgba(239,68,68,0.2)' }}>🗑️</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

// ─────────────────────────────────────────────────────────
// COMPONENTE: Carga Masiva (Lectora Óptica Simulada)
// ─────────────────────────────────────────────────────────
interface CargaMasivaViewProps {
  token: string;
}

function CargaMasivaView({ token }: CargaMasivaViewProps) {
  const [textoCarga, setTextoCarga] = useState('');
  const [loading, setLoading] = useState(false);
  const [mensajeResultado, setMensajeResultado] = useState<string | null>(null);

  const handleCargar = async () => {
    if (!textoCarga.trim()) {
      alert('Por favor ingresa datos en el cuadro de texto.');
      return;
    }

    const lineas = textoCarga.trim().split('\n');
    const payload: any[] = [];

    for (const linea of lineas) {
      const partes = linea.split(',');
      if (partes.length !== 2) continue;
      const dni = partes[0].trim();
      const respuestas = partes[1].trim();
      if (dni && respuestas.length === 100) {
        payload.push({ dni, respuestas });
      }
    }

    if (payload.length === 0) {
      alert('No se encontraron líneas válidas. Formato requerido: DNI,100_RESPUESTAS');
      return;
    }

    setLoading(true); setMensajeResultado(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/admision/carga-masiva`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
      });

      const data = await res.json();
      if (res.ok) {
        setMensajeResultado(`✅ Carga masiva exitosa: se procesaron e importaron las respuestas de ${data.procesados} postulantes.`);
        setTextoCarga('');
      } else {
        alert(data.mensaje || 'Error al procesar la carga masiva.');
      }
    } catch {
      alert('Error de conexión.');
    } finally {
      setLoading(false);
    }
  };

  const generarEjemplo = () => {
    const opciones = ['A', 'B', 'C', 'D'];
    const lineas: string[] = [];

    // Generar ejemplo para el usuario Prueba (12345678)
    let respsPrueba = '';
    for (let j = 0; j < 100; j++) {
      respsPrueba += opciones[Math.floor(Math.random() * 4)];
    }
    lineas.push(`12345678,${respsPrueba}`);

    // Generar algunos DNI correlativos
    for (let i = 1; i <= 5; i++) {
      const dni = (10000000 + i).toString();
      let resps = '';
      for (let j = 0; j < 100; j++) {
        resps += opciones[Math.floor(Math.random() * 4)];
      }
      lineas.push(`${dni},${resps}`);
    }

    setTextoCarga(lineas.join('\n'));
    setMensajeResultado(null);
  };

  return (
    <div className="glass-panel fade-in-up" style={{ padding: '28px' }}>
      <h3>📥 Carga Masiva desde Lectora Óptica</h3>
      <p style={{ color: 'var(--text-muted)', marginBottom: '20px' }}>
        Simula la entrada de datos de la lectora óptica. Pega la lista de DNI junto con la cadena de las 100 respuestas marcadas por cada postulante.
      </p>

      <div style={{ display: 'flex', gap: '20px', flexWrap: 'wrap', marginBottom: '20px' }}>
        <div style={{ flex: 2, minWidth: '300px' }}>
          <textarea
            value={textoCarga}
            onChange={e => setTextoCarga(e.target.value)}
            placeholder="Formato: DNI,RESPUESTAS&#10;Ejemplo:&#10;12345678,AABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCCDDAABBCC&#10;10000001,CCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDDCCDD"
            style={{ width: '100%', height: '300px', background: 'rgba(255,255,255,0.05)', color: 'white', border: '1px solid var(--glass-border)', borderRadius: '10px', padding: '16px', outline: 'none', fontFamily: 'monospace', fontSize: '0.85rem', lineHeight: '1.4' }}
          />
        </div>

        <div className="glass-panel" style={{ flex: 1, minWidth: '240px', padding: '20px', background: 'rgba(255,255,255,0.02)' }}>
          <h4 style={{ marginBottom: '12px' }}>📖 Instrucciones del Formato</h4>
          <ul style={{ fontSize: '0.82rem', color: 'var(--text-muted)', paddingLeft: '20px', display: 'flex', flexDirection: 'column', gap: '10px', marginBottom: '20px' }}>
            <li>Cada fila representa un postulante.</li>
            <li>El formato debe ser: <code>[DNI],[100 RESPUESTAS]</code>.</li>
            <li>No deben haber espacios alrededor de la coma.</li>
            <li>La cadena de respuestas debe tener exactamente <strong>100 caracteres</strong> (letras A, B, C o D).</li>
            <li>Cualquier otra letra se considerará respuesta omitida o incorrecta.</li>
          </ul>

          <button onClick={generarEjemplo} className="logout-btn" style={{ width: '100%', marginBottom: '10px' }}>
            📋 Generar datos de prueba
          </button>
        </div>
      </div>

      {mensajeResultado && (
        <div className="success-message" style={{ marginBottom: '20px' }}>
          {mensajeResultado}
        </div>
      )}

      <button onClick={handleCargar} className="primary-btn" disabled={loading} style={{ maxWidth: '300px' }}>
        {loading ? '⏳ Procesando carga...' : '🚀 Cargar y Evaluar Respuestas'}
      </button>
    </div>
  );
}

export default App;
