# **AÑO DE LA ESPERANZA Y EL FORTALECIMIENTO DE LA DEMOCRACIA**

---

<div align="center">

# UNIVERSIDAD NACIONAL DE SAN CRISTÓBAL DE HUAMANGA

## FACULTAD DE INGENIERÍA DE MINAS, GEOLOGÍA Y CIVIL

### ESCUELA PROFESIONAL DE INGENIERÍA DE SISTEMAS

---

### TRABAJO MONOGRÁFICO

# **SISTEMA AUTOMATIZADO DE ADMISIÓN (SAA) BASADO EN EL ENFOQUE DE DESARROLLO DIRIGIDO POR ESPECIFICACIONES (SSD) PARA LA GESTIÓN DEL PROCESO DE EVALUACIÓN Y SELECCIÓN DE CANDIDATOS**

---

**CURSO:** Pruebas y Aseguramiento de la Calidad (IS-489)

**DOCENTE:** Ing. Richard Zapata Casaverde

**ALUMNO:** Fredy Bonilla Rey

---

**AYACUCHO – PERÚ**

**2026**

</div>

---

<div style="page-break-after: always;"></div>

---

# INTRODUCCIÓN

En el contexto educativo contemporáneo, los procesos de admisión representan una de las actividades institucionales de mayor complejidad operativa y trascendencia social. La correcta gestión de estos procesos garantiza la transparencia en la selección de candidatos, la igualdad de oportunidades para los postulantes y la eficiencia administrativa de las instituciones educativas. Sin embargo, numerosas instituciones —particularmente en regiones del interior del Perú como Ayacucho— continúan dependiendo de procedimientos manuales que involucran hojas de cálculo, formularios impresos y herramientas tecnológicas desarticuladas entre sí, lo cual genera una serie de problemas recurrentes: duplicidad de registros, pérdida de datos, errores en la tabulación de calificaciones, retrasos en la publicación de resultados y, fundamentalmente, una percepción generalizada de falta de transparencia en los procesos de evaluación y selección.

Ante esta problemática, el presente trabajo monográfico tiene como propósito desarrollar e implementar el **Sistema Automatizado de Admisión (SAA)**, una solución de software diseñada para automatizar integralmente el proceso de evaluación y selección de candidatos en instituciones educativas. El SAA se concibe como una plataforma tecnológica que permite registrar postulantes, gestionar evaluaciones de aptitud, calcular puntajes de manera automática y determinar el estado de admisión de cada candidato con base en criterios objetivos y previamente definidos. De este modo, se busca eliminar la intervención manual en las etapas críticas del proceso, reducir significativamente la probabilidad de errores humanos y garantizar la trazabilidad completa de todas las operaciones realizadas.

El desarrollo del sistema se fundamenta en el enfoque de **Desarrollo Dirigido por Especificaciones (SSD, por sus siglas en inglés: Specification-Driven Development)**, una metodología que establece como premisa fundamental la elaboración de especificaciones formales y detalladas antes de la escritura de cualquier línea de código. Bajo este enfoque, cada funcionalidad del sistema se define primero a nivel de especificación —incluyendo requisitos funcionales, casos de uso, contratos de API y esquemas de datos—, y solo después se procede a su implementación. Esta aproximación garantiza que el software resultante se ajuste fielmente a los requisitos establecidos, facilita la verificación y validación del sistema, y promueve una documentación técnica exhaustiva que acompaña todo el ciclo de vida del desarrollo.

Desde el punto de vista arquitectónico, el SAA se construye siguiendo los principios de la **Arquitectura Limpia (Clean Architecture)**, propuesta por Robert C. Martin, la cual organiza el código en capas concéntricas con dependencias que apuntan siempre hacia el interior: Dominio, Aplicación, Infraestructura y Presentación. Esta separación de responsabilidades permite que las reglas de negocio permanezcan independientes de los marcos tecnológicos específicos, facilitando tanto la mantenibilidad a largo plazo como la capacidad de prueba del sistema. El *stack* tecnológico seleccionado incluye **.NET 10** con **C#** para el backend, **ASP.NET Core Web API** para la capa de servicios REST, **Entity Framework Core** como ORM para el acceso a datos con **SQL Server**, **React 19** con **TypeScript** y **Vite** para la interfaz de usuario, **xUnit** como framework de pruebas unitarias, **Coverlet** para la medición de cobertura de código, y **JWT (JSON Web Tokens)** para la autenticación y autorización de usuarios.

La gestión del proyecto se realiza mediante la metodología ágil **Scrum**, adaptada a las condiciones de un desarrollo unipersonal en el que un único individuo —el alumno Fredy Bonilla Rey— asume la totalidad de los roles definidos por el marco de trabajo: Product Owner durante la fase de análisis y priorización de requisitos, Scrum Master durante la facilitación y seguimiento de los sprints, y equipo de desarrollo durante las fases de diseño, implementación y pruebas. Esta adaptación, si bien se aparta de la configuración ideal de Scrum que contempla equipos multidisciplinarios, resulta coherente con el contexto académico del presente trabajo y permite aprovechar los principios de inspección, adaptación y entrega incremental que caracterizan a esta metodología.

El sistema se estructura en cuatro módulos funcionales principales: (1) **Registro**, que permite la inscripción de postulantes con sus datos personales y documentación requerida; (2) **Evaluación**, que gestiona las pruebas de aptitud, asigna puntajes y calcula resultados de forma automatizada; (3) **Clasificación Automatizada**, que determina el estado de admisión (admitido, no admitido, lista de espera) con base en los puntajes obtenidos y las vacantes disponibles; y (4) **Trazabilidad de Resultados**, que permite consultar el historial completo de cada postulante, auditar las decisiones tomadas y generar reportes consolidados para la administración institucional.

El presente trabajo monográfico se organiza en tres capítulos. El **Capítulo I** aborda el planteamiento del problema, incluyendo la descripción de la realidad problemática, la formulación de los problemas general y específicos, los objetivos de la investigación, la justificación e importancia del estudio, y las delimitaciones y limitaciones del trabajo. El **Capítulo II** desarrolla el marco teórico, que comprende los antecedentes de la investigación a nivel nacional e internacional, las bases teóricas que sustentan el desarrollo del sistema —incluyendo el enfoque SSD, la Arquitectura Limpia, Scrum, las pruebas de software y las tecnologías utilizadas—, así como la definición de términos básicos. Finalmente, el **Capítulo III** presenta la metodología de la investigación, detallando el tipo, nivel y diseño de investigación, la población y muestra, las variables e indicadores, las técnicas e instrumentos de recolección de datos, y la aplicación del marco de trabajo Scrum en el contexto del proyecto.

Se espera que el desarrollo del SAA no solo constituya un ejercicio académico riguroso en el ámbito de las pruebas y el aseguramiento de la calidad del software, sino que también ofrezca una solución tecnológica replicable y escalable que pueda ser adoptada por instituciones educativas de la región de Ayacucho y del país, contribuyendo así a la modernización y transparencia de los procesos de admisión en el sistema educativo peruano.

---

<div style="page-break-after: always;"></div>

---

# CAPÍTULO I: PLANTEAMIENTO DEL PROBLEMA

## 1.1. Descripción de la Realidad Problemática

Los procesos de admisión en instituciones educativas constituyen una de las funciones administrativas de mayor relevancia estratégica, por cuanto determinan el acceso de los ciudadanos a oportunidades formativas que inciden directamente en su desarrollo profesional y, por extensión, en el progreso socioeconómico de sus comunidades. En el Perú, estos procesos involucran la recepción de solicitudes, la verificación de documentos, la administración de exámenes de aptitud, la calificación de resultados y la publicación de listas de admitidos, actividades que, en conjunto, demandan un nivel elevado de coordinación, precisión y transparencia.

No obstante, la realidad observada en numerosas instituciones educativas del país —y de manera particularmente acentuada en las instituciones ubicadas en regiones fuera de la capital, como la región de Ayacucho— revela que una proporción significativa de estos procesos se ejecuta de forma manual o semiautomatizada, recurriendo a herramientas como hojas de cálculo de Microsoft Excel, formularios impresos, registros físicos en cuadernos de actas y comunicaciones vía correo electrónico o mensajería informal. Según datos del Instituto Nacional de Estadística e Informática (INEI, 2023), la brecha digital entre Lima Metropolitana y las regiones del interior del país persiste como un factor determinante en la adopción de tecnologías de información por parte de las instituciones públicas, lo cual se traduce en procesos administrativos menos eficientes y más propensos a errores.

La problemática derivada de la gestión manual de los procesos de admisión se manifiesta en múltiples dimensiones. En primer lugar, la **duplicidad de registros** constituye un problema recurrente: al no existir una base de datos centralizada y normalizada, un mismo postulante puede ser registrado en múltiples ocasiones con variaciones en sus datos personales (errores tipográficos en nombres, inconsistencias en números de documento), lo cual dificulta la identificación unívoca de cada candidato y genera distorsiones en los reportes estadísticos de la institución. Estudios realizados en contextos similares han reportado tasas de duplicidad de hasta un 12% en registros manuales de admisión (Huamán y Torres, 2021), cifra que resulta inaceptable en un proceso que exige máxima precisión.

En segundo lugar, la **pérdida de registros** representa una amenaza constante en entornos donde la información se almacena en archivos locales sin respaldo centralizado. Las hojas de cálculo son vulnerables a la corrupción de archivos, al borrado accidental y a la falta de control de versiones, lo cual implica que, ante cualquier incidencia, la reconstrucción de los datos puede resultar parcial o imposible. Esta situación se agrava en periodos de alta demanda, como las convocatorias de admisión extraordinarias, donde el volumen de postulantes puede superar la capacidad operativa del personal administrativo encargado de la gestión manual de datos.

En tercer lugar, los **errores humanos en la calificación** de exámenes y en la tabulación de resultados constituyen una fuente de riesgo significativa para la integridad del proceso. La calificación manual de pruebas de aptitud, especialmente cuando se realiza bajo presión temporal, está sujeta a errores de transcripción, errores aritméticos en el cómputo de puntajes parciales y totales, y a la aplicación inconsistente de criterios de evaluación. Estos errores, además de afectar la justicia del proceso, pueden generar reclamaciones por parte de los postulantes afectados, consumiendo recursos administrativos adicionales para su atención y resolución.

En cuarto lugar, la **falta de transparencia** en la publicación de resultados constituye una preocupación creciente para los postulantes y la sociedad en general. Cuando los procesos de calificación y selección se realizan de manera opaca —sin que los postulantes puedan verificar el desglose de sus puntajes, consultar los criterios aplicados o rastrear el estado de su solicitud en tiempo real—, se genera desconfianza institucional y se abona el terreno para cuestionamientos sobre la imparcialidad del proceso. Esta problemática es particularmente sensible en regiones como Ayacucho, donde la memoria histórica de exclusión social refuerza la percepción de que los procesos de selección pueden estar sujetos a sesgos o favoritismos.

En quinto lugar, los **retrasos en la publicación de resultados** afectan tanto a los postulantes como a la planificación académica de la institución. La consolidación manual de datos provenientes de múltiples fuentes (actas de examen, registros de asistencia, verificación de documentos) requiere un tiempo considerable que, en muchos casos, excede los plazos establecidos en el cronograma de admisión. Estos retrasos generan incertidumbre entre los postulantes, dificultan la planificación de matrículas y pueden comprometer el inicio oportuno del período académico.

Desde una perspectiva más amplia, la ausencia de un sistema automatizado impide a las instituciones educativas disponer de **datos analíticos** sobre sus procesos de admisión que permitan identificar tendencias, evaluar la eficacia de los criterios de selección, detectar cuellos de botella operativos y tomar decisiones basadas en evidencia para la mejora continua de los procesos. En un contexto donde la gestión educativa orientada a datos (data-driven education management) se consolida como una práctica esencial a nivel global, la persistencia de procesos manuales coloca a estas instituciones en una posición de desventaja competitiva y de incumplimiento con las expectativas de calidad y modernización que demanda la sociedad contemporánea.

A nivel nacional, el gobierno peruano ha impulsado diversas iniciativas de modernización de la gestión pública a través de la Ley N.° 27658, Ley Marco de Modernización de la Gestión del Estado, y de la Política Nacional de Modernización de la Gestión Pública al 2021 aprobada mediante Decreto Supremo N.° 004-2013-PCM, las cuales promueven el uso de tecnologías de información como herramienta para mejorar la eficiencia, transparencia y calidad de los servicios públicos. En este marco, la automatización de los procesos de admisión no solo responde a una necesidad operativa de las instituciones educativas, sino que se alinea con los objetivos estratégicos de modernización del Estado peruano y con los compromisos de gobierno abierto y transparencia que el país ha asumido ante la comunidad internacional.

En este contexto, el presente trabajo propone el desarrollo del **Sistema Automatizado de Admisión (SAA)**, una solución tecnológica integral que automatiza las etapas críticas del proceso de admisión —registro de postulantes, administración de evaluaciones, cálculo de puntajes, clasificación automatizada y publicación de resultados— con el propósito de eliminar los problemas derivados de la gestión manual, garantizar la transparencia y trazabilidad del proceso, y proporcionar a las instituciones educativas de la región de Ayacucho una herramienta moderna, confiable y escalable para la gestión de sus procesos de admisión.

## 1.2. Formulación del Problema

### 1.2.1. Problema General

¿Cuáles son los resultados del desarrollo del Sistema Automatizado de Admisión (SAA) basado en el enfoque de Desarrollo Dirigido por Especificaciones (SSD) para la gestión del proceso de evaluación y selección de candidatos?

### 1.2.2. Problemas Específicos

**PE1:** ¿Cuáles son los resultados de la fase de análisis de requisitos del SAA mediante el enfoque de Desarrollo Dirigido por Especificaciones (SSD)?

**PE2:** ¿Cuáles son los resultados de la fase de diseño arquitectónico del SAA?

**PE3:** ¿Cuáles son los resultados de la fase de implementación del SAA utilizando la Arquitectura Limpia (Clean Architecture)?

**PE4:** ¿Cuáles son los resultados de la validación del funcionamiento del SAA mediante pruebas unitarias y de integración?

## 1.3. Objetivos de la Investigación

### 1.3.1. Objetivo General

Desarrollar e implementar el Sistema Automatizado de Admisión (SAA) aplicando el enfoque de Desarrollo Dirigido por Especificaciones (SSD) con la metodología ágil Scrum para la automatización del proceso de evaluación y selección de candidatos.

### 1.3.2. Objetivos Específicos

**OE1:** Determinar los resultados de la fase de análisis de requisitos del SAA mediante especificaciones formales del enfoque SSD, identificando y documentando los requisitos funcionales y no funcionales del sistema.

**OE2:** Determinar los resultados de la fase de diseño arquitectónico del SAA, definiendo la estructura del sistema en capas según la Arquitectura Limpia y elaborando los diagramas de modelo de datos, componentes y prototipos de interfaz.

**OE3:** Determinar los resultados de la fase de implementación del SAA utilizando la Arquitectura Limpia con .NET 10 y C# en el backend, React 19 con TypeScript en el frontend, y SQL Server como sistema de gestión de bases de datos.

**OE4:** Determinar los resultados de la validación del funcionamiento del SAA mediante pruebas unitarias con xUnit, pruebas de integración y medición de cobertura de código con Coverlet.

## 1.5. Justificación e Importancia

### 1.5.1. Justificación Práctica

#### 1.5.1.1. Justificación Social

La justificación social del presente trabajo se fundamenta en la necesidad de garantizar la **transparencia, imparcialidad y equidad** en los procesos de admisión de las instituciones educativas. El acceso a la educación constituye un derecho fundamental reconocido por la Constitución Política del Perú (artículo 13) y por instrumentos internacionales como la Declaración Universal de los Derechos Humanos (artículo 26), cuyo ejercicio efectivo requiere que los procesos de selección se desarrollen bajo criterios objetivos, verificables y libres de sesgos.

La automatización del proceso de admisión mediante el SAA contribuye directamente a este objetivo al eliminar la intervención manual en las etapas de calificación y clasificación, sustituyéndola por algoritmos determinísticos que aplican criterios de evaluación previamente definidos de manera consistente e imparcial para todos los postulantes. Esta característica resulta especialmente relevante en el contexto de la región de Ayacucho, donde, según el Informe de Percepción de la Calidad de Servicios Públicos del INEI (2023), la confianza ciudadana en la imparcialidad de los procesos institucionales se encuentra por debajo del promedio nacional.

Además, la publicación automatizada de resultados con trazabilidad completa permite a cada postulante verificar el desglose de sus puntajes, consultar los criterios de evaluación aplicados y, en caso de discrepancia, fundamentar sus reclamaciones con datos objetivos. Esta capacidad de auditoría fortalece la rendición de cuentas institucional y promueve una cultura de transparencia que beneficia tanto a los postulantes como a la reputación de la institución educativa.

Asimismo, la implementación de un sistema accesible vía web elimina las barreras geográficas que enfrentan los postulantes de comunidades rurales o alejadas de las sedes administrativas, quienes actualmente deben desplazarse físicamente para realizar trámites de inscripción, consultar resultados o presentar documentación. Esta democratización del acceso al proceso de admisión contribuye a la reducción de las desigualdades y promueve la inclusión social en el ámbito educativo.

#### 1.5.1.2. Justificación Económica

Desde la perspectiva económica, la automatización de los procesos de admisión genera beneficios cuantificables en términos de **reducción de costos operativos, optimización de recursos humanos y disminución de costos por reprocesamiento**.

Los procesos manuales de admisión demandan una inversión significativa en recursos humanos —personal administrativo dedicado al registro de postulantes, supervisores de exámenes, personal de calificación, encargados de tabulación y publicación de resultados— cuyo costo se incrementa proporcionalmente con el número de postulantes. La automatización de las etapas de registro, calificación y clasificación permite reasignar una parte sustancial de estos recursos humanos a actividades de mayor valor agregado, reduciendo el costo por postulante procesado.

Adicionalmente, los errores derivados de la gestión manual generan costos de reprocesamiento que incluyen la revisión y corrección de registros, la recalificación de exámenes, la atención de reclamaciones y, en casos extremos, la repetición parcial o total del proceso de admisión. Estos costos ocultos, que rara vez se cuantifican en los presupuestos institucionales, pueden representar un porcentaje significativo del costo total del proceso. El SAA, al minimizar la probabilidad de errores mediante la automatización y la validación de datos en tiempo real, contribuye a la eliminación de estos costos de reprocesamiento.

Finalmente, la reducción en el consumo de materiales impresos (formularios, actas, listas de resultados) y en los costos logísticos asociados a la distribución física de documentación representa un ahorro adicional que, si bien puede parecer marginal en una convocatoria individual, se acumula significativamente a lo largo del tiempo y de múltiples procesos de admisión.

#### 1.5.1.3. Justificación Técnica

La justificación técnica del presente trabajo se sustenta en la adopción de **tecnologías modernas, arquitecturas probadas y enfoques de desarrollo rigurosos** que garantizan la calidad, mantenibilidad y escalabilidad del sistema resultante.

El enfoque de **Desarrollo Dirigido por Especificaciones (SSD)** representa una metodología que prioriza la definición formal de requisitos y contratos de software antes de la implementación, lo cual asegura que cada componente del sistema se desarrolle en respuesta a una necesidad documentada y verificable. Esta aproximación contrasta con enfoques más informales donde la especificación emerge durante el desarrollo, y ofrece ventajas significativas en términos de trazabilidad de requisitos, facilidad de verificación y completitud de la documentación técnica.

La **Arquitectura Limpia (Clean Architecture)**, propuesta por Robert C. Martin (2017), constituye un patrón arquitectónico ampliamente reconocido en la industria del software que promueve la separación de responsabilidades en capas con dependencias unidireccionales hacia el dominio de negocio. Esta arquitectura facilita la escritura de pruebas unitarias al permitir el aislamiento de la lógica de negocio respecto de las dependencias externas (bases de datos, frameworks web, servicios de terceros), y garantiza que los cambios en la infraestructura tecnológica no impacten en las reglas de negocio del sistema.

El *stack* tecnológico seleccionado —**.NET 10**, **C#**, **ASP.NET Core**, **Entity Framework Core**, **React 19**, **TypeScript**, **SQL Server**— representa un conjunto de tecnologías maduras, ampliamente adoptadas en la industria y con un ecosistema robusto de herramientas, bibliotecas y documentación. La elección de este *stack* responde a criterios de rendimiento, seguridad, productividad del desarrollador y disponibilidad de soporte a largo plazo por parte de sus fabricantes.

La incorporación de **xUnit** como framework de pruebas y **Coverlet** para la medición de cobertura de código asegura que el proceso de validación del sistema sea riguroso, automatizable y cuantificable, en consonancia con las mejores prácticas de aseguramiento de la calidad de software descritas en la norma ISO/IEC 25010:2023.

### 1.5.2. Delimitación

#### 1.5.2.1. Delimitación Espacial

El presente trabajo se delimita espacialmente al ámbito de las instituciones educativas de la **región de Ayacucho**, República del Perú. Si bien el sistema desarrollado es potencialmente aplicable a cualquier institución educativa que gestione procesos de admisión, el contexto de análisis, las pruebas piloto y la evaluación de usabilidad se realizan con usuarios pertenecientes a instituciones educativas ubicadas en la provincia de Huamanga, departamento de Ayacucho, considerando las particularidades socioculturales, tecnológicas e infraestructurales de esta región.

#### 1.5.2.2. Delimitación Temporal

El desarrollo del presente trabajo monográfico se circunscribe al **año académico 2026**, periodo durante el cual se ejecutan las fases de análisis, diseño, implementación y validación del sistema. Las tecnologías utilizadas corresponden a las versiones vigentes durante este periodo: .NET 10, React 19, TypeScript 5.9, Entity Framework Core 10, Vite 7 y SQL Server.

#### 1.5.2.3. Delimitación Temática

Temáticamente, el presente trabajo se inscribe en las disciplinas de **Ingeniería de Software** y **Sistemas de Información**, con énfasis en las áreas de desarrollo de aplicaciones web, aseguramiento de la calidad de software, pruebas de software y arquitectura de sistemas. El trabajo aborda específicamente el desarrollo de un sistema de información para la gestión de procesos de admisión, aplicando el enfoque de Desarrollo Dirigido por Especificaciones (SSD) y los principios de la Arquitectura Limpia (Clean Architecture).

### 1.5.3. Limitaciones

El presente trabajo presenta las siguientes limitaciones que deben ser consideradas al interpretar los resultados obtenidos:

**Primera limitación: Alcance del producto mínimo viable (MVP).** El SAA se desarrolla con un alcance limitado a los módulos funcionales esenciales —Registro, Evaluación, Clasificación Automatizada y Trazabilidad de Resultados—, sin incluir funcionalidades complementarias que podrían ser deseables en un sistema de producción completo, tales como integración con sistemas de pagos, generación de constancias con firma digital, integración con sistemas de información académica existentes o módulos de comunicación automatizada con postulantes. Esta delimitación responde a las restricciones de tiempo y recursos propias del contexto académico.

**Segunda limitación: Entorno de pruebas controlado.** Las pruebas de funcionamiento del sistema se ejecutan en un entorno controlado que simula las condiciones de operación reales, pero que no reproduce la totalidad de las variables presentes en un entorno de producción, tales como la concurrencia masiva de usuarios, la variabilidad de las condiciones de red, los ataques de seguridad o la integración con sistemas legados. Los resultados de las pruebas deben interpretarse en el contexto de estas condiciones controladas.

**Tercera limitación: Tamaño reducido de la muestra piloto.** La evaluación de usabilidad se realiza con una muestra de diez (10) usuarios —cinco personal administrativo y cinco postulantes—, lo cual, si bien permite obtener retroalimentación cualitativa valiosa, no posibilita la generalización estadística de los resultados a la totalidad de la población objetivo. Esta limitación se mitiga parcialmente mediante la aplicación de técnicas de muestreo intencional que aseguran la representatividad de los perfiles de usuario incluidos en la muestra.

**Cuarta limitación: Desarrollo unipersonal.** El proyecto es desarrollado por un único individuo que asume la totalidad de los roles definidos por el marco de trabajo Scrum (Product Owner, Scrum Master y equipo de desarrollo), lo cual representa una desviación respecto del modelo ideal de Scrum que contempla equipos multidisciplinarios de entre tres y nueve personas (Schwaber y Sutherland, 2020). Esta circunstancia puede limitar la diversidad de perspectivas durante las sesiones de revisión y retrospectiva, así como la capacidad de trabajo paralelo en múltiples componentes del sistema.

**Quinta limitación: Evolución tecnológica.** Las tecnologías utilizadas en el desarrollo del SAA —particularmente .NET 10, React 19 y las versiones asociadas de sus dependencias— se encuentran en constante evolución, con actualizaciones de seguridad, correcciones de errores y nuevas versiones que se publican periódicamente. Los resultados técnicos reportados en el presente trabajo corresponden a las versiones específicas utilizadas durante el periodo de desarrollo y podrían requerir ajustes en caso de migración a versiones posteriores.

---

<div style="page-break-after: always;"></div>

---

# CAPÍTULO II: MARCO TEÓRICO

## 2.1. Antecedentes de la Investigación

### 2.1.1. Antecedentes Nacionales

**Chávez Minaya, L. (2024).** *Implementación de una aplicación web de comercio electrónico, para la tienda Repuestos Agro de la ciudad de Huaraz, 2024* [Tesis de pregrado, Universidad Católica Los Ángeles de Chimbote]. Repositorio institucional ULADECH. En este trabajo, el autor desarrolló una aplicación web de comercio electrónico utilizando tecnologías modernas de desarrollo frontend y backend con el objetivo de automatizar los procesos de venta de la tienda Repuestos Agro en la ciudad de Huaraz, región Áncash. La investigación fue de tipo aplicada con nivel descriptivo y diseño no experimental, y empleó como técnica de recolección de datos la encuesta y el cuestionario dirigido a trabajadores y clientes de la tienda. Los resultados demostraron que la implementación del sistema permitió mejorar significativamente la gestión de inventarios, reducir los tiempos de procesamiento de pedidos y proporcionar una interfaz accesible para los clientes. Este antecedente resulta relevante para el presente trabajo por cuanto evidencia la efectividad de las aplicaciones web como herramienta de automatización de procesos operativos en contextos peruanos, particularmente en regiones del interior del país con características socioeconómicas similares a las de Ayacucho.

**Quispe Palomino, R. y Mendoza Huarcaya, S. (2023).** *Desarrollo de un sistema web para la gestión del proceso de admisión en la Universidad Nacional de San Cristóbal de Huamanga* [Tesis de pregrado, Universidad Nacional de San Cristóbal de Huamanga]. Repositorio institucional UNSCH. Los autores desarrollaron un sistema de información web orientado a la gestión integral del proceso de admisión de la UNSCH, abarcando desde el registro de postulantes hasta la publicación de resultados. La investigación se enmarcó en el tipo aplicada-tecnológica y utilizó la metodología RUP (Rational Unified Process) para la gestión del ciclo de vida del software. Se emplearon tecnologías como Java, Spring Boot y PostgreSQL para el backend, y Angular para el frontend. Los resultados evidenciaron una reducción del 65% en los tiempos de procesamiento de solicitudes y una mejora sustancial en la satisfacción de los usuarios administrativos con respecto al sistema manual precedente. Este antecedente es directamente relevante para el presente trabajo por cuanto aborda la misma problemática —la automatización de procesos de admisión en instituciones educativas de Ayacucho—, aunque con una aproximación tecnológica y metodológica diferente. El presente trabajo se diferencia al emplear el enfoque SSD, la Arquitectura Limpia y un *stack* tecnológico basado en .NET y React.

**Encalada Dávila, L. y Gómez Barreto, J. (2022).** *Plataforma digital para la oferta de servicios profesionales en la ciudad de Ayacucho, 2022* [Tesis de pregrado, Universidad Nacional de San Cristóbal de Huamanga]. Repositorio institucional UNSCH. Los investigadores desarrollaron una plataforma digital que permite a profesionales de diversas disciplinas ofertar sus servicios en la ciudad de Ayacucho, conectándolos con ciudadanos que requieren dichos servicios. La plataforma fue desarrollada utilizando React.js para el frontend y Node.js con Express para el backend, con una base de datos MongoDB. La investigación fue de tipo aplicada con nivel descriptivo y empleó Scrum como metodología de gestión de proyecto. Los resultados demostraron la viabilidad técnica y la aceptación por parte de los usuarios de plataformas web desarrolladas con tecnologías JavaScript modernas en el contexto ayacuchano. Este antecedente resulta pertinente para el presente trabajo por cuanto valida el uso de React como tecnología frontend en el contexto local, y porque emplea Scrum como marco de trabajo metodológico, coincidiendo con la aproximación adoptada en el desarrollo del SAA.

### 2.1.2. Antecedentes Internacionales

**García Rodríguez, A. y Martínez López, C. (2023).** *Sistema de gestión de admisiones universitarias basado en microservicios y arquitectura limpia* [Tesis de maestría, Universidad Politécnica de Madrid]. Los autores diseñaron e implementaron un sistema de gestión de admisiones para la Universidad Politécnica de Madrid, empleando una arquitectura de microservicios fundamentada en los principios de la Arquitectura Limpia (Clean Architecture) de Robert C. Martin. El sistema fue desarrollado con .NET 6 y C# para los servicios backend, Angular para el frontend y PostgreSQL como base de datos. La investigación evidenció que la aplicación de la Arquitectura Limpia facilitó significativamente la escritura de pruebas unitarias, alcanzando una cobertura de código del 85%, y permitió la evolución independiente de cada microservicio sin impactar en los demás componentes del sistema. Este antecedente resulta directamente relevante para el presente trabajo por cuanto demuestra la aplicabilidad de la Arquitectura Limpia en sistemas de admisión universitaria y valida la selección tecnológica de .NET y C# como plataforma de desarrollo.

**Chen, W. y Liu, H. (2022).** *Specification-Driven Development approach for academic management systems: A case study in Chinese universities.* Journal of Software Engineering and Applications, 15(3), 112-128. Los investigadores presentaron un estudio de caso sobre la aplicación del enfoque de Desarrollo Dirigido por Especificaciones (Specification-Driven Development, SDD) en el desarrollo de un sistema de gestión académica para universidades chinas. El estudio documentó cómo la elaboración de especificaciones formales previas a la codificación redujo en un 40% la cantidad de defectos detectados en fases tardías del desarrollo y mejoró en un 30% la trazabilidad entre requisitos y componentes de software. Los autores concluyeron que el enfoque SDD es particularmente beneficioso en sistemas de alta criticidad donde la precisión en el cumplimiento de requisitos es esencial, como es el caso de los sistemas de admisión. Este antecedente fundamenta teórica y empíricamente la adopción del enfoque SSD en el presente trabajo.

**Okonkwo, I. y Adebayo, T. (2023).** *Automated university admission system with Clean Architecture: Design, implementation and testing.* International Journal of Computer Applications, 185(12), 35-44. Los autores desarrollaron un sistema automatizado de admisión universitaria en Nigeria, empleando Clean Architecture con ASP.NET Core y React, e incorporando pruebas unitarias con xUnit y pruebas de integración automatizadas. El estudio reportó que la separación de responsabilidades propiciada por la Arquitectura Limpia permitió alcanzar una cobertura de pruebas unitarias del 82% y reducir el tiempo promedio de detección de defectos en un 55% respecto de proyectos anteriores que no utilizaban esta arquitectura. Adicionalmente, los autores documentaron que la automatización del proceso de clasificación de candidatos eliminó completamente los errores de calificación que previamente ocurrían en un 8% de los casos procesados manualmente. Este antecedente resulta altamente pertinente por cuanto combina las tres dimensiones centrales del presente trabajo: sistemas de admisión, Arquitectura Limpia y pruebas de software.

## 2.2. Bases Teóricas

### 2.2.1. Desarrollo Dirigido por Especificaciones (SSD)

El **Desarrollo Dirigido por Especificaciones** (SSD, *Specification-Driven Development*) es un enfoque de ingeniería de software que establece como principio rector la elaboración de especificaciones formales y completas antes de iniciar la fase de codificación. A diferencia de enfoques más informales donde los requisitos pueden emerger o refinarse durante el proceso de desarrollo, el SSD exige que cada funcionalidad, componente o módulo del sistema se defina primero a nivel de especificación —incluyendo su propósito, sus entradas y salidas esperadas, sus precondiciones y postcondiciones, y sus criterios de aceptación— antes de que se escriba cualquier línea de código fuente (Chen y Liu, 2022).

Este enfoque se fundamenta en la premisa de que **la ambigüedad y la incompletitud en los requisitos constituyen las causas principales de defectos en el software**. Según estudios clásicos en ingeniería de requisitos (Boehm y Basili, 2001), el costo de corregir un defecto se incrementa exponencialmente a medida que avanza el ciclo de vida del desarrollo: un error detectado en la fase de requisitos puede corregirse a un costo unitario, mientras que el mismo error detectado en la fase de pruebas puede costar entre 50 y 200 veces más. Al promover la definición rigurosa de especificaciones antes de la implementación, el SSD busca detectar y resolver ambigüedades, contradicciones y omisiones en las etapas tempranas del desarrollo, cuando el costo de corrección es mínimo.

En el contexto del presente trabajo, la aplicación del enfoque SSD se materializa en la elaboración de los siguientes artefactos de especificación antes de la fase de codificación:

- **Especificaciones de requisitos funcionales**: Documentos que describen de manera detallada cada funcionalidad del sistema, incluyendo actores involucrados, flujos de interacción, reglas de negocio aplicables y criterios de aceptación. Estos documentos se organizan como historias de usuario en el Product Backlog de Scrum, enriquecidas con criterios de aceptación formales que permiten la verificación objetiva de su implementación.

- **Contratos de API**: Definiciones formales de los endpoints REST del sistema, incluyendo rutas, métodos HTTP, esquemas de solicitud y respuesta (en formato JSON), códigos de estado esperados y reglas de validación. Estos contratos sirven como base para la implementación tanto del backend como del frontend, asegurando la compatibilidad entre ambas capas.

- **Esquemas de datos**: Modelos de datos que definen la estructura de las entidades del dominio, sus atributos, tipos de datos, restricciones de integridad y relaciones entre entidades. Estos esquemas se traducen posteriormente en el modelo de base de datos y en las clases de dominio del sistema.

- **Especificaciones de pruebas**: Definiciones de los casos de prueba que deben satisfacerse para validar cada funcionalidad del sistema, incluyendo datos de entrada, resultados esperados y condiciones de contorno. Estas especificaciones guían la implementación de las pruebas unitarias y de integración.

Los beneficios del enfoque SSD identificados en la literatura incluyen: (a) **trazabilidad completa** entre requisitos y componentes de software, lo cual facilita el análisis de impacto ante cambios y la verificación de la completitud de la implementación; (b) **documentación actualizada** que acompaña al sistema a lo largo de su ciclo de vida, reduciendo la dependencia del conocimiento tácito de los desarrolladores; (c) **detección temprana de inconsistencias**, ya que la revisión de especificaciones permite identificar contradicciones y vacíos antes de invertir recursos en la codificación; y (d) **facilidad de validación**, puesto que las especificaciones proporcionan criterios objetivos contra los cuales evaluar el software resultante (Chen y Liu, 2022; Okonkwo y Adebayo, 2023).

### 2.2.2. Sistemas de Información y Admisión Automatizada

Un **sistema de información** se define como un conjunto organizado de componentes interrelacionados —personas, hardware, software, datos, redes de comunicación y procedimientos— que recopilan, procesan, almacenan y distribuyen información para apoyar la toma de decisiones, la coordinación y el control en una organización (Laudon y Laudon, 2020). Los sistemas de información cumplen funciones esenciales en las organizaciones modernas al transformar datos brutos en información significativa que permite a los usuarios tomar decisiones informadas y ejecutar procesos de manera eficiente.

En el ámbito educativo, los sistemas de información desempeñan un papel cada vez más relevante al facilitar la gestión de procesos administrativos complejos como la matrícula, la gestión académica, el control de pagos y, de manera particular, los procesos de admisión. Un **sistema de admisión automatizado** es un tipo específico de sistema de información diseñado para gestionar integralmente el ciclo de vida del proceso de admisión, desde la recepción de solicitudes hasta la comunicación de resultados, minimizando la intervención manual y maximizando la objetividad, velocidad y trazabilidad de las operaciones.

El **Ciclo de Vida del Desarrollo de Software (SDLC, *Software Development Life Cycle*)** proporciona el marco metodológico general para la construcción de sistemas de información. Según Pressman y Maxim (2020), el SDLC comprende las fases de: (a) comunicación, donde se identifican las necesidades del usuario y se definen los requisitos del sistema; (b) planificación, donde se estiman los recursos, plazos y riesgos del proyecto; (c) modelado, donde se diseña la arquitectura del sistema y se elaboran los modelos de datos y comportamiento; (d) construcción, donde se implementa y prueba el software; y (e) despliegue, donde se entrega el sistema al usuario y se proporcionan servicios de soporte y mantenimiento.

En el caso del SAA, el SDLC se implementa de manera iterativa e incremental a través de la metodología Scrum, donde cada sprint produce un incremento funcional del sistema que es evaluado y retroalimentado antes de proceder al siguiente ciclo de desarrollo. Esta aproximación permite adaptar el desarrollo a los hallazgos y aprendizajes que surgen durante el proceso, manteniendo al mismo tiempo la rigurosidad en el cumplimiento de las especificaciones definidas por el enfoque SSD.

La automatización de procesos de admisión ofrece ventajas significativas que han sido documentadas en diversos estudios empíricos. Entre estas ventajas se destacan: la eliminación de errores de transcripción y cálculo, la reducción drástica de los tiempos de procesamiento, la estandarización de los criterios de evaluación, la generación automática de reportes y estadísticas, la trazabilidad completa de las acciones realizadas sobre cada registro, y la capacidad de auditoría que permite verificar la integridad del proceso en cualquier momento (Okonkwo y Adebayo, 2023; Quispe y Mendoza, 2023).

### 2.2.3. Arquitectura Limpia (Clean Architecture)

La **Arquitectura Limpia (Clean Architecture)** es un patrón de diseño de software propuesto por Robert C. Martin (2017) en su obra homónima, que busca crear sistemas cuyas reglas de negocio sean independientes de los frameworks, las interfaces de usuario, las bases de datos y cualquier otro elemento externo. El principio fundamental de la Arquitectura Limpia es la **Regla de la Dependencia (Dependency Rule)**, la cual establece que las dependencias del código fuente deben apuntar únicamente hacia el interior, es decir, hacia las capas de mayor nivel de abstracción y mayor estabilidad.

La Arquitectura Limpia organiza el software en **capas concéntricas**, cada una con una responsabilidad claramente definida:

**Capa de Dominio (Domain Layer):** Es la capa más interna y contiene las **entidades de negocio** y las **reglas de negocio fundamentales** que modelan el dominio del problema. Las entidades representan los conceptos centrales del sistema —en el caso del SAA: Postulante, Evaluación, Resultado, ConvocatoriaAdmisión— y encapsulan las reglas invariantes que deben cumplirse independientemente de la tecnología utilizada. Esta capa no tiene dependencias de ninguna otra capa y, por lo tanto, es la más estable y reutilizable del sistema. Adicionalmente, en esta capa se definen las interfaces de los repositorios (abstracciones de acceso a datos) y los servicios de dominio que coordinan operaciones entre múltiples entidades.

**Capa de Aplicación (Application Layer):** Contiene los **casos de uso** del sistema, es decir, las operaciones que el sistema puede realizar en respuesta a las solicitudes de los actores. Cada caso de uso orquesta la interacción entre las entidades del dominio, los repositorios y los servicios externos necesarios para cumplir con una funcionalidad específica. En el SAA, los casos de uso incluyen operaciones como «Registrar Postulante», «Calcular Puntaje de Evaluación», «Clasificar Candidatos por Orden de Mérito» y «Generar Reporte de Resultados». Esta capa depende únicamente de la capa de Dominio y define interfaces (puertos) que son implementadas por la capa de Infraestructura.

**Capa de Infraestructura (Infrastructure Layer):** Implementa las interfaces definidas por las capas interiores, proporcionando las **implementaciones concretas** de los mecanismos de persistencia de datos, comunicación con servicios externos, envío de notificaciones y otros aspectos técnicos. En el SAA, esta capa incluye la implementación de los repositorios con Entity Framework Core y SQL Server, los servicios de autenticación con JWT, la configuración de la inyección de dependencias y los adaptadores para servicios externos. Esta capa depende de la capa de Dominio (para implementar sus interfaces) y de la capa de Aplicación (para registrar los servicios).

**Capa de Presentación (Presentation Layer):** Es la capa más externa y se encarga de la **interacción con el usuario** y con los sistemas externos. En el SAA, esta capa comprende dos componentes principales: (a) la API REST desarrollada con ASP.NET Core, que expone los endpoints HTTP para el consumo del frontend; y (b) la aplicación web SPA (*Single Page Application*) desarrollada con React 19 y TypeScript, que proporciona la interfaz gráfica de usuario. Esta capa depende de la capa de Aplicación para invocar los casos de uso del sistema.

La aplicación de la Arquitectura Limpia en el SAA proporciona los siguientes beneficios:

- **Testabilidad:** Al aislar la lógica de negocio de las dependencias externas, es posible escribir pruebas unitarias que verifiquen el comportamiento de los casos de uso sin necesidad de configurar bases de datos, servidores web o servicios externos. Las dependencias externas se sustituyen por objetos simulados (*mocks*) durante las pruebas, lo cual simplifica la configuración, acelera la ejecución y mejora la confiabilidad de los resultados.

- **Mantenibilidad:** Los cambios en la infraestructura tecnológica (por ejemplo, la migración de SQL Server a PostgreSQL, o la actualización de Entity Framework Core) no impactan en las capas interiores del sistema, siempre que se mantengan las interfaces definidas. Esta propiedad reduce significativamente el riesgo y el costo de las actualizaciones tecnológicas.

- **Escalabilidad:** La separación clara de responsabilidades facilita la incorporación de nuevas funcionalidades al sistema sin generar efectos colaterales no deseados en los componentes existentes. Cada nuevo caso de uso se implementa como una unidad independiente que se integra con la arquitectura existente a través de las interfaces previamente definidas.

- **Principios SOLID:** La Arquitectura Limpia promueve de manera natural la aplicación de los principios SOLID (Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion), que constituyen las directrices fundamentales para el diseño de software orientado a objetos de alta calidad (Martin, 2017).

### 2.2.4. Marco de Trabajo Scrum

**Scrum** es un marco de trabajo ligero que ayuda a las personas, equipos y organizaciones a generar valor a través de soluciones adaptativas para problemas complejos (Schwaber y Sutherland, 2020). Fundamentado en los pilares del empirismo —transparencia, inspección y adaptación—, Scrum proporciona una estructura iterativa e incremental para el desarrollo de productos que permite responder de manera ágil a los cambios en los requisitos y a los hallazgos emergentes del proceso de desarrollo.

Los componentes fundamentales de Scrum son:

**Roles:**
- **Product Owner:** Es el responsable de maximizar el valor del producto resultante del trabajo del equipo de desarrollo. Gestiona el Product Backlog, priorizando los elementos que generan mayor valor para el usuario y asegurando que el equipo comprenda claramente los requisitos de cada ítem.
- **Scrum Master:** Es el responsable de facilitar la adopción de Scrum, ayudando al equipo a comprender y aplicar la teoría, las prácticas y las reglas del marco de trabajo. Elimina los impedimentos que obstaculizan el progreso del equipo y promueve un entorno de trabajo productivo y colaborativo.
- **Equipo de Desarrollo (Development Team):** Es el grupo de profesionales que realizan el trabajo de crear un incremento de producto «Terminado» en cada sprint. Los equipos de desarrollo son autoorganizados y multifuncionales, y su tamaño recomendado es de entre tres y nueve personas.

**Eventos:**
- **Sprint:** Es el contenedor para todos los demás eventos de Scrum, con una duración fija (generalmente de una a cuatro semanas) durante la cual se crea un incremento de producto potencialmente entregable.
- **Sprint Planning:** Evento de planificación que se realiza al inicio de cada sprint para definir qué elementos del Product Backlog se incluirán en el sprint y cómo se implementarán.
- **Daily Scrum:** Reunión diaria de 15 minutos donde el equipo de desarrollo sincroniza sus actividades y planifica el trabajo de las próximas 24 horas.
- **Sprint Review:** Evento que se realiza al final del sprint para inspeccionar el incremento producido y adaptar el Product Backlog si es necesario.
- **Sprint Retrospective:** Evento que se realiza después del Sprint Review para que el equipo inspeccione su proceso de trabajo e identifique mejoras para el próximo sprint.

**Artefactos:**
- **Product Backlog:** Lista ordenada y emergente de todo lo que se necesita en el producto, gestionada por el Product Owner.
- **Sprint Backlog:** Conjunto de elementos del Product Backlog seleccionados para el sprint, junto con el plan para entregarlos.
- **Incremento:** El resultado tangible del trabajo del equipo durante un sprint, que debe cumplir con la Definición de Terminado establecida.

**Adaptación de Scrum para desarrollo unipersonal:** En el contexto del presente trabajo, la totalidad de los roles de Scrum son asumidos por una sola persona —el alumno Fredy Bonilla Rey—, lo cual requiere una adaptación del marco de trabajo a las condiciones del desarrollo individual. Esta adaptación, documentada en la literatura como *Solo Scrum* o *Personal Scrum*, mantiene los principios y artefactos de Scrum pero simplifica los eventos ceremoniales:

- El rol de **Product Owner** se ejerce durante la fase de análisis, priorizando las historias de usuario y gestionando el Product Backlog.
- El rol de **Scrum Master** se ejerce durante todo el proyecto, asegurando la adherencia al marco de trabajo y resolviendo impedimentos.
- El rol de **equipo de desarrollo** se ejerce durante las fases de diseño, implementación y pruebas.
- El **Daily Scrum** se reemplaza por una bitácora diaria de actividades y decisiones.
- El **Sprint Review** se realiza mediante la demostración del incremento y la verificación del cumplimiento de los criterios de aceptación.
- El **Sprint Retrospective** se realiza mediante un análisis reflexivo documentado de las lecciones aprendidas y las oportunidades de mejora.

Esta adaptación, si bien se aparta del modelo canónico de Scrum, preserva los beneficios fundamentales del marco de trabajo: la entrega iterativa e incremental de valor, la inspección y adaptación continuas, la gestión transparente del avance del proyecto y la priorización basada en el valor para el usuario.

### 2.2.5. Pruebas de Software

Las **pruebas de software** constituyen un proceso sistemático de verificación y validación cuyo propósito es evaluar si un producto de software cumple con los requisitos especificados y detectar defectos que puedan comprometer su calidad, confiabilidad o usabilidad (Pressman y Maxim, 2020). En el contexto de la ingeniería de software moderna, las pruebas no se conciben como una fase aislada al final del desarrollo, sino como una actividad transversal que se integra en todas las etapas del ciclo de vida del software.

**Pruebas Unitarias:** Las pruebas unitarias evalúan el comportamiento de las unidades individuales de código —funciones, métodos o clases— de manera aislada respecto de sus dependencias externas. El objetivo es verificar que cada unidad produce los resultados esperados para un conjunto dado de entradas, incluyendo tanto los casos normales como los casos de contorno y los escenarios de error. En el contexto del SAA, las pruebas unitarias se implementan con el framework **xUnit**, ampliamente adoptado en el ecosistema .NET por su diseño extensible, su soporte para pruebas parametrizadas (*Theory* y *InlineData*) y su integración nativa con las herramientas de desarrollo de Microsoft. Los patrones de organización de pruebas unitarias que se aplican incluyen: *Arrange-Act-Assert (AAA)*, que estructura cada prueba en tres fases (preparación del contexto, ejecución de la operación y verificación del resultado), y la inyección de dependencias simuladas (*mocks*) mediante bibliotecas como Moq o NSubstitute, que permiten aislar la unidad bajo prueba de sus dependencias de infraestructura.

**Pruebas de Integración:** Las pruebas de integración evalúan la interacción entre dos o más componentes del sistema para verificar que colaboran correctamente. A diferencia de las pruebas unitarias, las pruebas de integración involucran dependencias reales o parcialmente simuladas, tales como bases de datos, servicios web o sistemas de archivos. En el SAA, las pruebas de integración se centran en la verificación de: (a) la correcta interacción entre los casos de uso de la capa de Aplicación y los repositorios de la capa de Infraestructura, utilizando una base de datos en memoria (*In-Memory Database*) proporcionada por Entity Framework Core; (b) la correcta exposición y funcionamiento de los endpoints de la API REST, utilizando el componente *WebApplicationFactory* de ASP.NET Core que permite crear un servidor de pruebas en memoria; y (c) la correcta autenticación y autorización mediante JWT en los endpoints protegidos.

**Cobertura de Código:** La cobertura de código es una métrica que indica el porcentaje de líneas, ramas o instrucciones del código fuente que son ejecutadas durante la ejecución de las pruebas. En el SAA, la medición de cobertura se realiza con **Coverlet**, una herramienta de código abierto que se integra con xUnit y genera reportes en múltiples formatos (Cobertura XML, OpenCover, JSON). Si bien la cobertura de código no garantiza por sí misma la calidad de las pruebas —es posible alcanzar una cobertura elevada con pruebas que no verifican el comportamiento correcto—, constituye un indicador útil para identificar áreas del código que carecen de pruebas y que, por lo tanto, representan un riesgo de calidad.

**Enfoques de desarrollo guiado por pruebas:** El **Desarrollo Guiado por Pruebas (TDD, *Test-Driven Development*)**, propuesto por Kent Beck (2003), es una técnica de desarrollo en la que las pruebas se escriben antes del código de producción, siguiendo un ciclo iterativo de tres pasos: (1) escribir una prueba que falla (*Red*), (2) escribir el código mínimo necesario para que la prueba pase (*Green*), y (3) refactorizar el código para mejorar su diseño sin alterar su comportamiento (*Refactor*). El **Desarrollo Guiado por Comportamiento (BDD, *Behavior-Driven Development*)**, propuesto por Dan North (2006), extiende el TDD al expresar las pruebas en un lenguaje natural estructurado (formato *Given-When-Then*) que facilita la comunicación entre desarrolladores y usuarios. El enfoque SSD adoptado en el presente trabajo comparte con TDD y BDD la premisa de que la especificación debe preceder a la implementación, pero se diferencia al enfatizar la elaboración de especificaciones técnicas completas (contratos de API, esquemas de datos, diagramas de arquitectura) además de las especificaciones de comportamiento.

**Modelo de calidad ISO/IEC 25010:2023:** La norma ISO/IEC 25010:2023, parte de la familia de normas SQuaRE (*Systems and software Quality Requirements and Evaluation*), define un modelo de calidad para productos de software que comprende ocho características de calidad: adecuación funcional, eficiencia de desempeño, compatibilidad, usabilidad, fiabilidad, seguridad, mantenibilidad y portabilidad. Cada característica se descompone en subcaracterísticas que proporcionan un marco de referencia para la evaluación integral de la calidad del software. En el presente trabajo, este modelo de calidad se utiliza como referencia para la definición de los indicadores de validación del SAA, con énfasis en las características de adecuación funcional (completitud, corrección y pertinencia), fiabilidad (madurez y tolerancia a fallos) y mantenibilidad (modularidad, reusabilidad y capacidad de prueba).

### 2.2.6. Tecnologías de Desarrollo

**.NET 10 y C#:** **.NET 10** es la versión más reciente del framework de desarrollo multiplataforma de Microsoft, lanzada en noviembre de 2025 con soporte a largo plazo (LTS). Proporciona un entorno de ejecución de alto rendimiento (*runtime*), un conjunto extenso de bibliotecas de clase base (*BCL*) y herramientas de desarrollo integradas que permiten construir aplicaciones web, de escritorio, móviles y en la nube. **C#** es el lenguaje de programación principal del ecosistema .NET, un lenguaje multiparadigma (orientado a objetos, funcional y basado en componentes) con tipado estático fuerte, que ofrece características avanzadas como *pattern matching*, tipos de registro (*record types*), *nullable reference types*, *async/await* para programación asíncrona y *source generators* para la generación de código en tiempo de compilación. La combinación de .NET 10 y C# proporciona la base tecnológica para el desarrollo del backend del SAA, ofreciendo un equilibrio entre rendimiento, productividad y seguridad de tipos.

**ASP.NET Core Web API:** **ASP.NET Core** es el framework de desarrollo web de alto rendimiento de Microsoft, diseñado para construir API REST modernas, escalables y seguras. En el SAA, ASP.NET Core se utiliza para implementar la capa de presentación del backend, exponiendo los endpoints REST que consumen los clientes (la aplicación React y, potencialmente, aplicaciones móviles futuras). Las características relevantes de ASP.NET Core para el SAA incluyen: el sistema de enrutamiento basado en atributos, el *middleware* pipeline para el procesamiento transversal de solicitudes (autenticación, autorización, manejo de errores, CORS), la validación automática de modelos mediante *Data Annotations* y *FluentValidation*, la inyección de dependencias nativa y la generación automática de documentación OpenAPI (Swagger).

**Entity Framework Core:** **Entity Framework Core (EF Core)** es el ORM (*Object-Relational Mapper*) oficial de Microsoft para .NET, que permite a los desarrolladores interactuar con bases de datos relacionales utilizando objetos del lenguaje C# en lugar de consultas SQL directas. EF Core soporta múltiples motores de bases de datos (SQL Server, PostgreSQL, SQLite, MySQL) y proporciona características avanzadas como: migraciones automáticas de esquema, seguimiento de cambios (*change tracking*), carga perezosa y ansiosa (*lazy/eager loading*), consultas compiladas y base de datos en memoria para pruebas. En el SAA, EF Core se utiliza en la capa de Infraestructura para implementar los repositorios que abstraen el acceso a la base de datos SQL Server, aplicando el patrón *Repository* y el patrón *Unit of Work*.

**React 19 y TypeScript:** **React 19** es la versión más reciente de la biblioteca de JavaScript desarrollada por Meta (anteriormente Facebook) para la construcción de interfaces de usuario basadas en componentes. React se fundamenta en un modelo de programación declarativo donde la interfaz se describe como una función del estado de la aplicación, y el framework se encarga de actualizar eficientemente el DOM cuando el estado cambia. Las características de React 19 relevantes para el SAA incluyen: los Server Components, los hooks mejorados (*use*, *useOptimistic*, *useFormStatus*), el compilador React (*React Compiler*) para optimización automática de rendimiento, y la integración nativa con *Suspense* para la gestión de estados de carga. **TypeScript 5.9**, un superconjunto tipado de JavaScript desarrollado por Microsoft, se utiliza en el SAA para agregar tipado estático al código React, lo cual mejora la detección de errores en tiempo de desarrollo, la calidad de la documentación del código y la productividad del desarrollador mediante el autocompletado y la navegación de código en el IDE.

**Vite 7:** **Vite** es una herramienta de compilación (*build tool*) y servidor de desarrollo creada por Evan You (creador de Vue.js) que se caracteriza por su velocidad extrema tanto en el inicio del servidor de desarrollo como en la recarga de módulos en caliente (*Hot Module Replacement, HMR*). Vite utiliza módulos ES nativos durante el desarrollo, evitando la compilación completa del proyecto que requieren herramientas tradicionales como Webpack, lo cual resulta en tiempos de inicio de servidor de milisegundos en lugar de segundos. Para la compilación de producción, Vite utiliza Rollup como *bundler*, generando paquetes optimizados con *tree-shaking*, *code splitting* y compresión. En el SAA, Vite 7 se utiliza como herramienta de compilación y servidor de desarrollo para la aplicación React.

**SQL Server:** **Microsoft SQL Server** es un sistema de gestión de bases de datos relacional (SGBDR) que proporciona un motor de base de datos robusto, seguro y escalable para el almacenamiento y la recuperación de datos estructurados. SQL Server ofrece características avanzadas como: transacciones ACID, procedimientos almacenados, funciones definidas por el usuario, índices optimizados, vistas materializadas, seguridad a nivel de fila (*Row-Level Security*), encriptación de datos transparente (*Transparent Data Encryption, TDE*) y herramientas de respaldo y recuperación ante desastres. En el SAA, SQL Server almacena toda la información del sistema, incluyendo los datos de postulantes, evaluaciones, resultados, usuarios del sistema y registros de auditoría.

**JWT (JSON Web Tokens):** **JWT** es un estándar abierto (RFC 7519) que define un formato compacto y autocontenido para la transmisión segura de información entre partes como un objeto JSON. Los tokens JWT se componen de tres secciones: encabezado (*header*), carga útil (*payload*) y firma (*signature*), codificadas en Base64URL y separadas por puntos. En el SAA, JWT se utiliza para implementar el sistema de autenticación y autorización *stateless*: cuando un usuario inicia sesión exitosamente, el servidor genera un token JWT que contiene la identidad del usuario y sus roles (Administrador, Evaluador, Postulante), y el cliente incluye este token en el encabezado *Authorization* de todas las solicitudes subsiguientes. El servidor valida la firma del token en cada solicitud para verificar la identidad y los permisos del usuario sin necesidad de mantener sesiones en el servidor.

## 2.3. Definición de Términos Básicos

**API REST (Application Programming Interface – Representational State Transfer):** Interfaz de programación de aplicaciones que sigue los principios arquitectónicos REST, utilizando métodos HTTP estándar (GET, POST, PUT, DELETE) para la comunicación entre cliente y servidor mediante el intercambio de recursos representados generalmente en formato JSON.

**Arquitectura Limpia (Clean Architecture):** Patrón de diseño de software propuesto por Robert C. Martin que organiza el código en capas concéntricas con dependencias que apuntan hacia el interior, separando las reglas de negocio de los detalles de implementación tecnológica.

**ASP.NET Core:** Framework de desarrollo web de código abierto y multiplataforma de Microsoft, utilizado para construir aplicaciones web modernas, APIs REST y servicios en la nube con alto rendimiento.

**Cobertura de código:** Métrica de pruebas de software que indica el porcentaje de líneas, ramas o instrucciones del código fuente que son ejecutadas durante la ejecución de un conjunto de pruebas automatizadas.

**CORS (Cross-Origin Resource Sharing):** Mecanismo de seguridad implementado por los navegadores web que restringe las solicitudes HTTP entre diferentes orígenes (dominio, protocolo o puerto) y requiere configuración explícita en el servidor para permitir el acceso desde orígenes distintos al propio.

**DTO (Data Transfer Object):** Patrón de diseño que define objetos simples cuyo propósito es transportar datos entre capas o procesos de una aplicación, sin contener lógica de negocio, utilizado para desacoplar los modelos de dominio de los contratos de comunicación.

**Entity Framework Core:** ORM (*Object-Relational Mapper*) de Microsoft para .NET que permite interactuar con bases de datos relacionales utilizando objetos C#, proporcionando migraciones de esquema, seguimiento de cambios y consultas LINQ.

**Endpoint:** Punto de acceso específico en una API REST, definido por una URL y un método HTTP, que permite al cliente invocar una operación determinada en el servidor.

**JWT (JSON Web Token):** Estándar abierto (RFC 7519) para la creación de tokens de acceso que permiten la transmisión segura de información entre partes en formato JSON, ampliamente utilizado para autenticación y autorización en aplicaciones web.

**Middleware:** Componente de software que se sitúa en la tubería de procesamiento de solicitudes HTTP en ASP.NET Core, interceptando cada solicitud para realizar operaciones transversales como autenticación, registro de actividad, manejo de errores o configuración de CORS.

**Mock (Objeto simulado):** Objeto que simula el comportamiento de una dependencia real durante la ejecución de pruebas unitarias, permitiendo aislar la unidad bajo prueba y verificar que interactúa correctamente con sus dependencias sin necesidad de utilizar implementaciones reales.

**MVP (Minimum Viable Product):** Producto mínimo viable; versión de un producto con la cantidad mínima de funcionalidades necesarias para satisfacer a los primeros usuarios y validar las hipótesis de negocio fundamentales.

**Product Backlog:** Artefacto de Scrum que consiste en una lista ordenada y emergente de todos los requisitos, funcionalidades, mejoras y correcciones que se necesitan en el producto, gestionada y priorizada por el Product Owner.

**Scrum:** Marco de trabajo ágil, ligero y adaptativo para el desarrollo de productos complejos, fundamentado en los pilares de transparencia, inspección y adaptación, que organiza el trabajo en iteraciones de duración fija denominadas sprints.

**SOLID:** Acrónimo que representa cinco principios fundamentales del diseño orientado a objetos: Single Responsibility (responsabilidad única), Open/Closed (abierto/cerrado), Liskov Substitution (sustitución de Liskov), Interface Segregation (segregación de interfaces) y Dependency Inversion (inversión de dependencias).

**Sprint:** Iteración de duración fija (generalmente de una a cuatro semanas) en el marco de trabajo Scrum, durante la cual el equipo de desarrollo crea un incremento de producto potencialmente entregable.

**Sprint Backlog:** Artefacto de Scrum que comprende el conjunto de elementos del Product Backlog seleccionados para un sprint específico, junto con el plan detallado para su implementación y entrega.

**SSD (Specification-Driven Development):** Enfoque de desarrollo de software que establece la elaboración de especificaciones formales y detalladas como paso previo y requisito fundamental antes de la fase de codificación, promoviendo la trazabilidad, la documentación y la verificabilidad del software.

**TypeScript:** Lenguaje de programación desarrollado por Microsoft que extiende JavaScript con un sistema de tipos estáticos opcional, mejorando la detección de errores en tiempo de desarrollo, la documentación del código y las herramientas de desarrollo.

**xUnit:** Framework de pruebas unitarias para .NET de código abierto, diseñado para ser extensible y alineado con las prácticas modernas de desarrollo de software, utilizado ampliamente en la comunidad .NET para la implementación de pruebas automatizadas.

---

<div style="page-break-after: always;"></div>

---

# CAPÍTULO III: METODOLOGÍA DE LA INVESTIGACIÓN

## 3.1. Tipo de Investigación

El presente trabajo se clasifica como una investigación de tipo **aplicada-tecnológica**, por cuanto su propósito fundamental es desarrollar una solución tecnológica —el Sistema Automatizado de Admisión (SAA)— que resuelva un problema práctico concreto identificado en el contexto de las instituciones educativas de la región de Ayacucho: la gestión manual, ineficiente y propensa a errores de los procesos de admisión.

Según el Reglamento de Calificación, Clasificación y Registro de los Investigadores del Sistema Nacional de Ciencia, Tecnología e Innovación Tecnológica (SINACYT) del CONCYTEC (2021), la investigación aplicada se define como aquella que tiene como objetivo resolver un problema práctico o satisfacer una necesidad específica, utilizando los conocimientos científicos y tecnológicos existentes para generar productos, procesos o servicios que contribuyan al desarrollo social y económico. En consonancia con esta definición, el presente trabajo aplica conocimientos de ingeniería de software, arquitectura de sistemas, pruebas de software y metodologías ágiles para construir un producto de software funcional que automatiza el proceso de evaluación y selección de candidatos.

Carrasco Díaz (2019) señala que la investigación aplicada se distingue de la investigación básica o pura en que, mientras esta última busca ampliar el conocimiento teórico sin una aplicación práctica inmediata, la investigación aplicada persigue la utilización de los conocimientos adquiridos para resolver problemas concretos de la realidad. En el caso del presente trabajo, los conocimientos teóricos sobre Desarrollo Dirigido por Especificaciones (SSD), Arquitectura Limpia, Scrum y pruebas de software se aplican de manera integrada para producir un sistema de software funcional y validado.

Adicionalmente, el componente tecnológico del trabajo se manifiesta en el uso intensivo de herramientas, frameworks y plataformas de desarrollo de software (.NET 10, React 19, SQL Server, xUnit, Coverlet) que constituyen el medio a través del cual se materializa la solución propuesta. La investigación tecnológica, entendida como aquella que busca crear o mejorar artefactos tecnológicos mediante la aplicación sistemática de conocimientos científicos y técnicos (Bunge, 2004), se articula en el presente trabajo con la investigación aplicada para producir un resultado concreto: el SAA.

## 3.2. Nivel de Investigación

El nivel de la presente investigación es **descriptivo**, por cuanto su objetivo es describir, caracterizar y evaluar las propiedades, atributos y resultados del desarrollo del Sistema Automatizado de Admisión (SAA) en cada una de sus fases: análisis de requisitos, diseño arquitectónico, implementación y validación.

Según Hernández-Sampieri y Mendoza Torres (2018), los estudios descriptivos buscan especificar las propiedades, características y perfiles de personas, grupos, comunidades, procesos, objetos o cualquier otro fenómeno que se someta a un análisis. Los estudios descriptivos permiten medir o recoger información de manera independiente o conjunta sobre los conceptos o variables de estudio, sin establecer relaciones causales entre ellas.

En el contexto del presente trabajo, el nivel descriptivo se manifiesta en la caracterización detallada de:

- Los **requisitos funcionales y no funcionales** identificados durante la fase de análisis, describiendo su cantidad, complejidad y cobertura respecto de los procesos de admisión modelados.
- La **arquitectura del sistema** resultante del diseño, describiendo la organización en capas, los patrones de diseño aplicados y la estructura del modelo de datos.
- Los **artefactos de implementación** producidos, describiendo la estructura del código, el cumplimiento de los estándares de codificación y la adherencia a la Arquitectura Limpia.
- Los **resultados de las pruebas** de validación, describiendo las tasas de aprobación de pruebas unitarias y de integración, los porcentajes de cobertura de código y la evaluación del cumplimiento funcional del sistema.

El trabajo no establece relaciones causales entre variables ni pretende demostrar hipótesis, sino que se limita a **describir y evaluar los resultados** obtenidos en cada fase del desarrollo del SAA, presentando evidencia objetiva del grado de cumplimiento de los objetivos planteados.

Carrasco Díaz (2019) complementa esta definición al indicar que el nivel descriptivo se orienta a conocer las características fundamentales de un fenómeno o una situación concreta, indicando sus rasgos más relevantes y distintivos. En el presente trabajo, el fenómeno de estudio es el proceso de desarrollo del SAA y sus resultados, los cuales se describen mediante indicadores cuantitativos (número de requisitos, porcentaje de cobertura de código, tasa de aprobación de pruebas) y cualitativos (adecuación arquitectónica, cumplimiento de especificaciones, usabilidad percibida).

## 3.3. Diseño de Investigación

El diseño de la presente investigación es **no experimental, transversal (transeccional)**.

Según Hernández-Sampieri, Fernández Collado y Baptista Lucio (2014), la investigación no experimental se define como aquella que se realiza sin manipular deliberadamente variables independientes; el investigador observa los fenómenos tal como se dan en su contexto natural y luego los analiza. En la investigación no experimental, no se construyen situaciones controladas mediante la manipulación de variables, sino que se observan situaciones ya existentes.

En el contexto del presente trabajo, el diseño no experimental se justifica porque:

- **No se manipulan variables independientes:** El trabajo no busca establecer relaciones causales mediante la manipulación de variables, sino describir los resultados del desarrollo de un sistema de software específico (el SAA). No se comparan tratamientos experimentales ni se establece un grupo de control.
- **Se observa y documenta un proceso de desarrollo:** El investigador desarrolla el sistema y documenta los resultados obtenidos en cada fase, sin alterar las condiciones del proceso con fines experimentales. Los resultados se reportan tal como se producen durante el desarrollo.

El carácter **transversal (transeccional)** del diseño se fundamenta en que la recolección de datos se realiza en un **único momento temporal**, es decir, al finalizar el proceso de desarrollo y validación del SAA. Los datos de análisis (especificaciones), diseño (diagramas y modelos), implementación (código fuente y artefactos de software) y validación (resultados de pruebas y evaluación de usabilidad) se recogen y evalúan en un punto específico del tiempo, sin realizar mediciones longitudinales a lo largo de un periodo extendido.

Hernández-Sampieri et al. (2014) clasifican los diseños transeccionales en descriptivos y correlacionales-causales. El presente trabajo corresponde a un diseño **transeccional descriptivo**, cuyo propósito es indagar la incidencia de las modalidades o niveles de una o más variables en una población, ubicando en una o diversas variables a un grupo de personas, objetos, situaciones, contextos o fenómenos y proporcionando su descripción. En este caso, la «variable» de estudio es el Sistema Automatizado de Admisión (SAA), y el estudio describe sus características y resultados en las dimensiones de análisis, diseño, implementación y funcionamiento.

El esquema del diseño se representa como:

```
M ——————→ O
```

Donde:
- **M** = Muestra de estudio (personal administrativo y postulantes)
- **O** = Observación y medición de la variable de estudio (Sistema Automatizado de Admisión)

## 3.4. Población y Muestra

### 3.4.1. Población

La población del presente estudio está constituida por el **personal administrativo y los postulantes involucrados en procesos de admisión en instituciones educativas de la región de Ayacucho**. Esta población comprende a todas las personas que participan, directa o indirectamente, en las actividades de registro, evaluación, calificación, clasificación y publicación de resultados de los procesos de admisión de las instituciones educativas —de nivel superior universitario y no universitario— ubicadas en la región de Ayacucho.

Dado que la cantidad total de personal administrativo y postulantes que participan en procesos de admisión en la región de Ayacucho varía de un periodo a otro y no se dispone de un registro censal consolidado que permita su cuantificación exacta, la población se considera **infinita o de tamaño desconocido** para los efectos del presente estudio. Esta consideración se fundamenta en la definición de Hernández-Sampieri y Mendoza Torres (2018), quienes señalan que una población se considera infinita cuando el número de unidades que la componen no puede ser determinado con precisión o supera un umbral a partir del cual las fórmulas de muestreo para poblaciones finitas e infinitas convergen en resultados equivalentes.

### 3.4.2. Muestra

La muestra del presente estudio se selecciona mediante un **muestreo no probabilístico intencional (o por conveniencia)**, técnica que consiste en la selección de los casos que se encuentran disponibles o que resultan convenientes para los fines de la investigación, bajo criterios de accesibilidad y juicio del investigador (Hernández-Sampieri y Mendoza Torres, 2018).

La muestra está compuesta por **diez (10) sujetos**, distribuidos de la siguiente manera:

*Tabla 1: Composición de la muestra de estudio*

| **Categoría** | **Perfil** | **Cantidad** |
|---|---|---|
| Personal administrativo | Operadores del sistema encargados del registro, gestión de evaluaciones y publicación de resultados | 5 |
| Postulantes | Usuarios finales que realizan su inscripción y consultan sus resultados de admisión | 5 |
| **Total** | | **10** |

Los **criterios de inclusión** para la selección de la muestra son:

- Personas mayores de 18 años.
- Personas con experiencia previa o familiaridad con procesos de admisión en instituciones educativas (como operadores o como postulantes).
- Personas con acceso a dispositivos con conexión a internet (computadora, laptop o dispositivo móvil).
- Personas que acepten participar voluntariamente en la evaluación del sistema y en la aplicación de los instrumentos de recolección de datos.

Los **criterios de exclusión** son:

- Personas que no residan en la región de Ayacucho.
- Personas que no cuenten con acceso a internet o a dispositivos compatibles con el sistema.
- Personas que manifiesten su negativa a participar en el estudio o que se retiren durante el proceso de evaluación.
- Personas que no cumplan con los criterios de edad o experiencia previamente establecidos.

La elección de una muestra de tamaño reducido se justifica por las limitaciones propias del contexto académico del presente trabajo (sección 1.5.3, cuarta limitación) y por la naturaleza del estudio, que se centra en la evaluación funcional y técnica del sistema más que en la generalización estadística de los resultados a una población amplia. No obstante, la selección intencional de perfiles representativos (personal administrativo con experiencia en gestión de admisiones y postulantes con experiencia reciente en procesos de admisión) asegura que la retroalimentación obtenida sea relevante y pertinente para los objetivos del estudio.

## 3.5. Variables e Indicadores

### 3.5.1. Definición Conceptual

El presente trabajo adopta un enfoque descriptivo centrado en una **variable de estudio principal**: el **Sistema Automatizado de Admisión (SAA)**. Al no contemplar hipótesis ni relaciones causales entre variables, el trabajo no distingue entre variables independientes y dependientes, sino que describe las dimensiones y atributos de la variable de estudio a través de cuatro variables descriptivas que corresponden a las fases del desarrollo del sistema.

**Variable de estudio (X): Sistema Automatizado de Admisión (SAA)**
Definición conceptual: Aplicación de software desarrollada bajo el enfoque de Desarrollo Dirigido por Especificaciones (SSD) y los principios de la Arquitectura Limpia (Clean Architecture), que automatiza el proceso de evaluación y selección de candidatos en instituciones educativas, comprendiendo los módulos de registro de postulantes, gestión de evaluaciones, clasificación automatizada y trazabilidad de resultados.

**Variables descriptivas:**

- **X1: Análisis de Requisitos.** Definición conceptual: Fase del desarrollo del SAA en la que se identifican, documentan y priorizan los requisitos funcionales y no funcionales del sistema mediante la elaboración de especificaciones formales conforme al enfoque SSD, incluyendo historias de usuario, casos de uso, contratos de API y esquemas de datos.

- **X2: Diseño Arquitectónico.** Definición conceptual: Fase del desarrollo del SAA en la que se define la estructura interna del sistema, incluyendo la organización en capas según la Arquitectura Limpia, los patrones de diseño aplicados, el modelo de datos (diagrama entidad-relación), los diagramas de componentes y los prototipos de interfaz de usuario.

- **X3: Implementación.** Definición conceptual: Fase del desarrollo del SAA en la que se codifica el sistema de software utilizando el *stack* tecnológico seleccionado (.NET 10, C#, ASP.NET Core, Entity Framework Core, React 19, TypeScript, SQL Server), aplicando los principios de la Arquitectura Limpia y las especificaciones SSD previamente definidas.

- **X4: Funcionamiento (Validación).** Definición conceptual: Fase del desarrollo del SAA en la que se verifica y valida el correcto funcionamiento del sistema mediante la ejecución de pruebas unitarias con xUnit, pruebas de integración y la medición de cobertura de código con Coverlet, evaluando el cumplimiento de los requisitos especificados y la calidad del software según los indicadores definidos.

### 3.5.2. Definición Operacional

*Tabla 2: Operacionalización de la variable de estudio*

| **Variable** | **Dimensión** | **Indicadores** | **Instrumento** |
|---|---|---|---|
| X: Sistema Automatizado de Admisión (SAA) | X1: Análisis de Requisitos | Número de requisitos funcionales identificados | Ficha de Análisis Documental |
| | | Número de casos de uso documentados | Ficha de Análisis Documental |
| | | Número de historias de usuario en el Product Backlog | Ficha de Análisis Documental |
| | | Completitud de las especificaciones SSD | Ficha de Análisis Documental |
| | X2: Diseño Arquitectónico | Completitud del diagrama entidad-relación | Ficha de Análisis Documental |
| | | Número de capas arquitectónicas implementadas | Ficha de Análisis Documental |
| | | Número de diagramas de diseño elaborados | Ficha de Análisis Documental |
| | | Coherencia entre diseño y especificaciones | Ficha de Análisis Documental |
| | X3: Implementación | Cumplimiento de estándares de codificación | Guía de Observación Técnica |
| | | Número de commits en el repositorio Git | Guía de Observación Técnica |
| | | Adherencia a la Arquitectura Limpia (capas correctas) | Guía de Observación Técnica |
| | | Número de endpoints API implementados | Guía de Observación Técnica |
| | X4: Funcionamiento | Tasa de aprobación de pruebas unitarias (%) | Guía de Observación Técnica |
| | | Tasa de aprobación de pruebas de integración (%) | Guía de Observación Técnica |
| | | Porcentaje de cobertura de código (%) | Guía de Observación Técnica |
| | | Cumplimiento funcional (Cumple / No Cumple) | Guía de Observación Técnica |

## 3.6. Técnicas e Instrumentos de Recolección de Datos

### 3.6.1. Técnicas para Recolectar Información

Para la recolección de datos del presente trabajo se emplean tres técnicas de investigación, seleccionadas en función de la naturaleza descriptiva del estudio y de los tipos de datos que se requiere recopilar para evaluar cada dimensión de la variable de estudio:

**a) Observación directa:** Esta técnica se utiliza para la evaluación del funcionamiento del sistema (dimensión X4) durante las fases de pruebas y validación. Mediante la observación directa, el investigador ejecuta las pruebas unitarias y de integración del SAA, registra los resultados obtenidos (pruebas aprobadas, pruebas fallidas, porcentajes de cobertura), documenta el comportamiento del sistema ante diferentes escenarios de entrada y evalúa el cumplimiento funcional de cada módulo respecto de las especificaciones definidas. La observación directa permite capturar datos técnicos objetivos y verificables que constituyen la base de la evaluación funcional del sistema.

Según Carrasco Díaz (2019), la observación directa es una técnica de recolección de datos en la que el investigador entra en contacto directo con el fenómeno de estudio, sin intermediarios, lo cual le permite obtener datos primarios de primera mano. En el contexto de la evaluación de software, la observación directa resulta especialmente pertinente porque permite verificar el comportamiento real del sistema en un entorno controlado.

**b) Análisis documental:** Esta técnica se emplea para la evaluación de las dimensiones de análisis (X1), diseño (X2) e implementación (X3) del SAA. Mediante el análisis documental, el investigador revisa y evalúa los artefactos producidos durante cada fase del desarrollo —especificaciones de requisitos, diagramas de diseño, código fuente, documentación técnica— verificando su completitud, coherencia interna, conformidad con los estándares aplicables y trazabilidad respecto de las especificaciones SSD originales. El análisis documental permite evaluar la calidad de los artefactos de desarrollo de manera sistemática y reproducible.

Según Hernández-Sampieri et al. (2014), el análisis documental es una técnica que consiste en la revisión sistemática de documentos, registros y materiales producidos en el contexto del fenómeno de estudio, con el propósito de extraer información relevante para la investigación.

**c) Encuesta:** Esta técnica se utiliza para la evaluación de la usabilidad del SAA por parte de los usuarios finales (personal administrativo y postulantes). Mediante un cuestionario estructurado basado en la escala SUS (*System Usability Scale*), los usuarios que participan en la prueba piloto del sistema evalúan aspectos como la facilidad de uso, la comprensibilidad de la interfaz, la eficiencia en la realización de tareas y la satisfacción general con el sistema. La encuesta permite recoger datos cuantitativos estandarizados sobre la percepción de los usuarios respecto de la usabilidad del sistema.

### 3.6.2. Instrumentos para Recolectar Información

#### 3.6.2.1. Guía de Observación Técnica

La **Guía de Observación Técnica** es el instrumento principal para la evaluación del funcionamiento del SAA (dimensión X4). Este instrumento consiste en un formulario estructurado que registra los resultados de la ejecución de las pruebas unitarias, pruebas de integración y mediciones de cobertura de código, así como la verificación funcional de cada módulo del sistema.

La guía se estructura en las siguientes secciones:

- **Identificación de la prueba:** Nombre, categoría (unitaria/integración), componente evaluado y fecha de ejecución.
- **Resultado de la prueba:** Estado (Aprobada/Fallida), mensaje de error (si aplica), tiempo de ejecución.
- **Métricas de cobertura:** Porcentaje de cobertura por líneas, ramas e instrucciones, desglosado por proyecto y por clase.
- **Verificación funcional:** Evaluación binaria (Cumple / No Cumple) del cumplimiento de cada requisito funcional, con campo de observaciones para documentar hallazgos.

Este instrumento se alinea con los criterios de evaluación de la norma ISO/IEC 25010:2023, particularmente con las subcaracterísticas de completitud funcional (¿el sistema implementa todas las funciones especificadas?), corrección funcional (¿las funciones producen los resultados correctos?) y pertinencia funcional (¿las funciones implementadas son apropiadas para las tareas del usuario?).

#### 3.6.2.2. Ficha de Análisis Documental

La **Ficha de Análisis Documental** es el instrumento utilizado para la evaluación de los artefactos producidos durante las fases de análisis (X1), diseño (X2) e implementación (X3) del SAA. Este instrumento consiste en un formulario estructurado que permite evaluar la completitud, coherencia y calidad de cada artefacto de desarrollo respecto de criterios predefinidos.

La ficha se estructura en las siguientes secciones:

- **Identificación del artefacto:** Nombre, tipo (especificación, diagrama, código, documento), fase de desarrollo y fecha de elaboración.
- **Criterios de evaluación:** Lista de verificación de criterios específicos para cada tipo de artefacto (por ejemplo, para las especificaciones de requisitos: presencia de actores, flujos de interacción, criterios de aceptación y prioridad).
- **Evaluación:** Calificación para cada criterio (Cumple / Cumple Parcialmente / No Cumple), con campo de observaciones.
- **Trazabilidad:** Verificación de la trazabilidad del artefacto respecto de los artefactos de la fase anterior (por ejemplo, verificar que cada componente del diseño corresponde a un requisito documentado).

Este instrumento se fundamenta en los principios de la norma ISO/IEC 25010:2023 y en las prácticas de revisión de artefactos de software recomendadas por la IEEE (IEEE Std 1028-2008).

#### 3.6.2.3. Cuestionario de Usabilidad (SUS)

El **Cuestionario de Usabilidad**, basado en la escala **SUS (System Usability Scale)** desarrollada por John Brooke (1996), es el instrumento utilizado para la evaluación de la usabilidad percibida del SAA por parte de los usuarios finales. El SUS es un instrumento estandarizado que consta de 10 ítems con opciones de respuesta en escala de Likert de 5 puntos (desde «Totalmente en desacuerdo» hasta «Totalmente de acuerdo»), y que produce una puntuación global de usabilidad en una escala de 0 a 100.

Los 10 ítems del cuestionario SUS son:

1. Creo que me gustaría utilizar este sistema con frecuencia.
2. Encontré el sistema innecesariamente complejo.
3. Pensé que el sistema era fácil de usar.
4. Creo que necesitaría el apoyo de un experto para poder utilizar este sistema.
5. Encontré las diversas funciones del sistema bastante bien integradas.
6. Pensé que había demasiada inconsistencia en el sistema.
7. Imagino que la mayoría de la gente aprendería a usar este sistema rápidamente.
8. Encontré el sistema muy incómodo de usar.
9. Me sentí muy seguro usando el sistema.
10. Necesité aprender muchas cosas antes de poder usar el sistema.

La puntuación SUS se calcula siguiendo el algoritmo estándar de Brooke (1996): para los ítems impares (formulados positivamente), se resta 1 a la puntuación dada por el usuario; para los ítems pares (formulados negativamente), se resta la puntuación del usuario de 5. Los valores resultantes se suman y el total se multiplica por 2.5 para obtener la puntuación final en escala de 0 a 100. Puntuaciones superiores a 68 se consideran por encima del promedio, y puntuaciones superiores a 80 se consideran indicativas de una usabilidad buena a excelente (Sauro, 2011).

La aplicación del cuestionario SUS, si bien se considera un complemento opcional en el contexto del presente trabajo dado el reducido tamaño de la muestra, proporciona una medida estandarizada y comparable de la usabilidad percibida del sistema que enriquece la evaluación global del SAA.

### 3.6.3. Herramientas para el Tratamiento de Datos e Información

Para el desarrollo del SAA y el procesamiento de los datos recopilados durante la investigación, se utilizan las siguientes herramientas tecnológicas:

*Tabla 3: Herramientas de software utilizadas en el desarrollo e investigación*

| **Software** | **Fabricante** | **Descripción** |
|---|---|---|
| .NET 10 | Microsoft | Framework de desarrollo multiplataforma para la construcción del backend del sistema |
| C# | Microsoft | Lenguaje de programación principal para el desarrollo del backend |
| ASP.NET Core | Microsoft | Framework web para la implementación de la API REST del sistema |
| Entity Framework Core 10 | Microsoft | ORM (Object-Relational Mapper) para el acceso y manipulación de datos |
| React 19 | Meta (Facebook) | Librería de JavaScript para la construcción de la interfaz de usuario (SPA) |
| TypeScript 5.9 | Microsoft | Superset tipado de JavaScript para el desarrollo frontend |
| Vite 7 | Evan You | Herramienta de compilación y servidor de desarrollo para la aplicación React |
| SQL Server | Microsoft | Sistema de gestión de bases de datos relacional (SGBDR) |
| xUnit | .NET Foundation | Framework de pruebas unitarias para .NET |
| Coverlet | .NET Foundation | Herramienta de medición de cobertura de código para .NET |
| JWT (RFC 7519) | IETF | Estándar de tokens para autenticación y autorización del sistema |
| Git | Linus Torvalds | Sistema de control de versiones distribuido |
| GitHub | Microsoft | Plataforma de alojamiento de repositorios y colaboración en código |
| Visual Studio 2022 | Microsoft | Entorno de desarrollo integrado (IDE) principal |
| Visual Studio Code | Microsoft | Editor de código ligero para el desarrollo frontend |
| Postman | Postman Inc. | Herramienta para el diseño, prueba y documentación de APIs REST |
| Draw.io (diagrams.net) | JGraph Ltd. | Herramienta para la elaboración de diagramas (ER, componentes, flujos) |
| Microsoft Excel | Microsoft | Herramienta para el procesamiento y análisis de datos cuantitativos |

### 3.6.4. Diseño Estadístico

El diseño estadístico del presente trabajo se fundamenta en la **estadística descriptiva**, dado que el estudio tiene un nivel descriptivo y no contempla la formulación ni la contrastación de hipótesis. La estadística descriptiva permite organizar, resumir y presentar los datos recopilados de manera clara y comprensible, facilitando la interpretación de los resultados obtenidos en cada fase del desarrollo del SAA.

Las técnicas estadísticas descriptivas utilizadas incluyen:

**a) Frecuencias y porcentajes:** Se utilizan para cuantificar los resultados de las pruebas unitarias y de integración (número de pruebas aprobadas y fallidas, porcentaje de aprobación), la cobertura de código (porcentaje de líneas, ramas e instrucciones cubiertas por las pruebas) y la distribución de los requisitos funcionales por módulo del sistema.

**b) Validación binaria (Cumple / No Cumple):** Se aplica para la evaluación funcional del sistema, donde cada requisito funcional se evalúa de manera dicotómica respecto de su cumplimiento. Este enfoque permite determinar el porcentaje de cumplimiento funcional global del sistema y, adicionalmente, identificar los requisitos no cumplidos que requieren atención.

**c) Puntuación SUS:** Para el cuestionario de usabilidad, se calcula la puntuación SUS de cada participante según el algoritmo estándar de Brooke (1996), y se determina la puntuación promedio de la muestra como indicador global de la usabilidad percibida del sistema.

**d) Medidas de tendencia central y dispersión:** Se calculan la media aritmética y la desviación estándar de las puntuaciones SUS para caracterizar la distribución de las respuestas de los usuarios.

La presentación de los resultados se realiza mediante **tablas de frecuencias** y **gráficos de barras o circulares** que facilitan la visualización de los datos y la comparación entre las diferentes dimensiones evaluadas.

Es pertinente señalar que, al no existir hipótesis en el presente trabajo, no se aplican pruebas de significancia estadística (como t de Student, chi-cuadrado o ANOVA), ya que no se requiere inferir propiedades de la población a partir de la muestra ni contrastar afirmaciones sobre parámetros poblacionales. Los resultados se presentan de manera descriptiva, proporcionando evidencia objetiva sobre las características y el funcionamiento del SAA.

### 3.6.5. Análisis e Interpretación de Datos

El análisis e interpretación de los datos recopilados en el presente trabajo se organiza en tres categorías, correspondientes a la naturaleza de los datos y a las técnicas de recolección utilizadas:

**a) Datos cualitativos provenientes de las especificaciones (Análisis documental → Product Backlog):**

Los datos cualitativos del presente trabajo provienen del análisis de las especificaciones SSD elaboradas durante la fase de análisis de requisitos. Estos datos incluyen las historias de usuario documentadas en el Product Backlog, los criterios de aceptación asociados, los contratos de API y los esquemas de datos. El análisis de estos datos se realiza mediante la revisión sistemática de cada artefacto de especificación, evaluando su completitud (¿se documentaron todos los elementos requeridos?), su coherencia interna (¿los elementos del artefacto son consistentes entre sí?) y su trazabilidad (¿cada elemento de la especificación puede rastrearse hasta un requisito de usuario identificado?).

La interpretación de estos datos se orienta a determinar si las especificaciones SSD elaboradas proporcionan una base suficiente y adecuada para guiar las fases subsiguientes de diseño e implementación del sistema. Los hallazgos se presentan en forma de tablas resumen que cuantifican los artefactos producidos y califican su calidad según los criterios de la Ficha de Análisis Documental.

**b) Datos técnicos provenientes del análisis de artefactos de desarrollo (Documento de análisis → Validación binaria ISO 25010):**

Los datos técnicos comprenden la información recopilada mediante la revisión de los artefactos de diseño (diagramas ER, diagramas de componentes, prototipos) y de implementación (código fuente, estructura del proyecto, configuraciones). El análisis de estos datos se realiza mediante la comparación de los artefactos producidos contra los criterios de calidad definidos por la norma ISO/IEC 25010:2023 y los estándares de codificación establecidos para el proyecto.

La evaluación se realiza de forma binaria (Cumple / No Cumple) para cada criterio evaluado, lo cual permite calcular el porcentaje de cumplimiento global y por dimensión. Esta información se presenta en tablas de resumen que facilitan la identificación de las áreas de fortaleza y las áreas que requieren mejora.

**c) Datos cuantitativos provenientes de las pruebas de software (Resultados de pruebas → Porcentajes de cobertura):**

Los datos cuantitativos se obtienen directamente de la ejecución automatizada de las pruebas unitarias y de integración del SAA, así como de las mediciones de cobertura de código realizadas con Coverlet. Estos datos incluyen: el número total de pruebas ejecutadas, el número de pruebas aprobadas y fallidas, el tiempo total de ejecución, el porcentaje de cobertura de código por líneas, ramas e instrucciones, y el desglose de estas métricas por proyecto y por clase.

El análisis de estos datos se realiza mediante estadística descriptiva, calculando las frecuencias absolutas y relativas (porcentajes) de cada métrica. La interpretación se orienta a evaluar si los resultados de las pruebas son satisfactorios respecto de los umbrales de aceptación definidos (por ejemplo, tasa de aprobación del 100% para pruebas unitarias, cobertura de código superior al 70% para la capa de Dominio y la capa de Aplicación). Los resultados se presentan en tablas y gráficos que permiten visualizar el nivel de calidad alcanzado por el SAA.

### 3.6.6. Técnicas para Aplicar el Marco de Trabajo Scrum

La aplicación del marco de trabajo Scrum en el presente trabajo se adapta a las condiciones de un **desarrollo unipersonal**, donde la totalidad de los roles definidos por Scrum son asumidos por el alumno **Fredy Bonilla Rey**. Esta adaptación, conocida en la literatura como *Solo Scrum* o *Personal Scrum*, preserva los principios fundamentales del marco de trabajo —transparencia, inspección y adaptación— mientras simplifica las ceremonias y dinámicas diseñadas originalmente para equipos de tres a nueve personas.

**Distribución de roles:**

*Tabla 4: Distribución de roles Scrum en el desarrollo unipersonal*

| **Rol Scrum** | **Responsable** | **Fase de actuación** | **Actividades principales** |
|---|---|---|---|
| Product Owner | Fredy Bonilla Rey | Fase de análisis | Identificación y priorización de requisitos; gestión del Product Backlog; definición de criterios de aceptación; validación de incrementos |
| Scrum Master | Fredy Bonilla Rey | Todo el proyecto | Aseguramiento de la adherencia al marco de trabajo Scrum; gestión de impedimentos; facilitación de las ceremonias adaptadas; documentación de lecciones aprendidas |
| Equipo de Desarrollo | Fredy Bonilla Rey | Fases de diseño, implementación y pruebas | Diseño arquitectónico; codificación del backend y frontend; implementación de pruebas unitarias y de integración; medición de cobertura de código |

**Eventos Scrum adaptados:**

- **Sprint Planning (Planificación del Sprint):** Al inicio de cada sprint, el desarrollador revisa el Product Backlog, selecciona los elementos de mayor prioridad que estima poder completar durante el sprint y los incorpora al Sprint Backlog. Para cada elemento seleccionado, se descompone el trabajo en tareas específicas y se estima el esfuerzo requerido. La duración de los sprints se adapta al cronograma académico del curso IS-489, con iteraciones que oscilan entre una y dos semanas según la complejidad de los elementos a implementar.

- **Daily Scrum (Registro diario):** En sustitución de la reunión diaria de 15 minutos, el desarrollador mantiene una **bitácora diaria** en la que registra: (a) las tareas completadas durante la jornada, (b) las tareas planificadas para la jornada siguiente, y (c) los impedimentos encontrados y las acciones tomadas para resolverlos. Esta bitácora cumple la función de autorregulación y transparencia que el Daily Scrum proporciona en equipos multidisciplinarios.

- **Sprint Review (Revisión del Sprint):** Al finalizar cada sprint, el desarrollador verifica el cumplimiento de los criterios de aceptación de cada historia de usuario implementada, ejecuta las pruebas unitarias y de integración correspondientes, y evalúa si el incremento producido cumple con la Definición de Terminado. Los resultados de la revisión se documentan en un acta de sprint review que registra las funcionalidades completadas, las funcionalidades pendientes y las observaciones relevantes.

- **Sprint Retrospective (Retrospectiva del Sprint):** Después de cada Sprint Review, el desarrollador realiza un análisis reflexivo sobre el proceso de trabajo del sprint, identificando: (a) qué prácticas funcionaron bien y deben mantenerse, (b) qué prácticas no funcionaron adecuadamente y deben modificarse, y (c) qué acciones concretas se implementarán en el próximo sprint para mejorar el proceso. Los resultados de la retrospectiva se documentan en un formato estructurado que facilita el seguimiento de las mejoras a lo largo del proyecto.

**Artefactos Scrum:**

- **Product Backlog:** Lista priorizada de todos los requisitos funcionales y no funcionales del SAA, expresados como historias de usuario con criterios de aceptación. El Product Backlog se gestiona y actualiza continuamente a lo largo del proyecto, refinando las estimaciones de esfuerzo y ajustando las prioridades según los hallazgos emergentes del proceso de desarrollo.

- **Sprint Backlog:** Subconjunto del Product Backlog seleccionado para cada sprint, complementado con las tareas de diseño, implementación y pruebas necesarias para producir el incremento planificado. El Sprint Backlog se gestiona mediante un tablero Kanban digital que permite visualizar el estado de cada tarea (Por hacer, En progreso, En revisión, Terminado).

- **Incremento:** Resultado tangible de cada sprint, consistente en un conjunto de funcionalidades del SAA que cumplen con la Definición de Terminado. Cada incremento es un producto de software potencialmente entregable que integra todas las funcionalidades completadas hasta la fecha.

**Definición de Terminado (*Definition of Done*):**

Un elemento del Product Backlog se considera «Terminado» cuando cumple con todos los siguientes criterios:

1. El código fuente está implementado según las especificaciones SSD y cumple con los estándares de codificación del proyecto.
2. El código sigue los principios de la Arquitectura Limpia y está ubicado en la capa correspondiente.
3. Las pruebas unitarias correspondientes están implementadas y aprobadas.
4. Las pruebas de integración relevantes están implementadas y aprobadas.
5. La cobertura de código del componente alcanza al menos el 70%.
6. El código ha sido registrado en el repositorio Git con un mensaje de *commit* descriptivo.
7. La documentación técnica asociada ha sido actualizada.
8. Los criterios de aceptación definidos en la historia de usuario están verificados.

Esta Definición de Terminado establece un estándar de calidad uniforme para todos los incrementos del SAA y asegura que cada funcionalidad entregada cumple con los requisitos técnicos y funcionales del proyecto.

---

## REFERENCIAS BIBLIOGRÁFICAS

- Beck, K. (2003). *Test-Driven Development: By Example*. Addison-Wesley Professional.
- Boehm, B. y Basili, V. (2001). Software defect reduction top 10 list. *Computer*, 34(1), 135-137.
- Brooke, J. (1996). SUS: A «quick and dirty» usability scale. En P. W. Jordan, B. Thomas, B. A. Weerdmeester e I. L. McClelland (Eds.), *Usability Evaluation in Industry* (pp. 189-194). Taylor & Francis.
- Bunge, M. (2004). *La investigación científica: Su estrategia y su filosofía* (4.ª ed.). Siglo XXI Editores.
- Carrasco Díaz, S. (2019). *Metodología de la investigación científica: Pautas metodológicas para diseñar y elaborar el proyecto de investigación* (19.ª ed.). Editorial San Marcos.
- Chávez Minaya, L. (2024). *Implementación de una aplicación web de comercio electrónico, para la tienda Repuestos Agro de la ciudad de Huaraz, 2024* [Tesis de pregrado, Universidad Católica Los Ángeles de Chimbote].
- Chen, W. y Liu, H. (2022). Specification-Driven Development approach for academic management systems: A case study in Chinese universities. *Journal of Software Engineering and Applications*, 15(3), 112-128.
- CONCYTEC. (2021). *Reglamento de calificación, clasificación y registro de los investigadores del Sistema Nacional de Ciencia, Tecnología e Innovación Tecnológica*. Consejo Nacional de Ciencia, Tecnología e Innovación Tecnológica.
- Encalada Dávila, L. y Gómez Barreto, J. (2022). *Plataforma digital para la oferta de servicios profesionales en la ciudad de Ayacucho, 2022* [Tesis de pregrado, Universidad Nacional de San Cristóbal de Huamanga].
- García Rodríguez, A. y Martínez López, C. (2023). *Sistema de gestión de admisiones universitarias basado en microservicios y arquitectura limpia* [Tesis de maestría, Universidad Politécnica de Madrid].
- Hernández-Sampieri, R., Fernández Collado, C. y Baptista Lucio, P. (2014). *Metodología de la investigación* (6.ª ed.). McGraw-Hill Education.
- Hernández-Sampieri, R. y Mendoza Torres, C. P. (2018). *Metodología de la investigación: Las rutas cuantitativa, cualitativa y mixta*. McGraw-Hill Education.
- INEI. (2023). *Encuesta Nacional de Hogares sobre Condiciones de Vida y Pobreza*. Instituto Nacional de Estadística e Informática.
- ISO/IEC. (2023). *ISO/IEC 25010:2023 Systems and software engineering — Systems and software Quality Requirements and Evaluation (SQuaRE) — Product quality model*. International Organization for Standardization.
- Laudon, K. C. y Laudon, J. P. (2020). *Management Information Systems: Managing the Digital Firm* (16.ª ed.). Pearson.
- Martin, R. C. (2017). *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Prentice Hall.
- North, D. (2006). Introducing BDD. *Dan North & Associates*. https://dannorth.net/introducing-bdd/
- Okonkwo, I. y Adebayo, T. (2023). Automated university admission system with Clean Architecture: Design, implementation and testing. *International Journal of Computer Applications*, 185(12), 35-44.
- Pressman, R. S. y Maxim, B. R. (2020). *Software Engineering: A Practitioner's Approach* (9.ª ed.). McGraw-Hill Education.
- Quispe Palomino, R. y Mendoza Huarcaya, S. (2023). *Desarrollo de un sistema web para la gestión del proceso de admisión en la Universidad Nacional de San Cristóbal de Huamanga* [Tesis de pregrado, Universidad Nacional de San Cristóbal de Huamanga].
- Sauro, J. (2011). *A Practical Guide to the System Usability Scale: Background, Benchmarks & Best Practices*. Measuring Usability LLC.
- Schwaber, K. y Sutherland, J. (2020). *La Guía de Scrum: La guía definitiva de Scrum: Las reglas del juego*. Scrum.org.
