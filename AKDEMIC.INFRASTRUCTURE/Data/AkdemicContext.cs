using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionLaboratorio;
using AKDEMIC.DOMAIN.Entities.InvestigacionPublicacion;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Extensions;
using AKDEMIC.INFRASTRUCTURE.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AKDEMIC.INFRASTRUCTURE.Data
{
    public class AkdemicContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<AuditEntry> _temporaryPropertyAuditEntries;

        public AkdemicContext(DbContextOptions<AkdemicContext> options, bool testEnv = false) : base(options)
        {
            _httpContextAccessor = new HttpContextAccessor();
            _temporaryPropertyAuditEntries = new List<AuditEntry>();
            if (!testEnv)
                Database.SetCommandTimeout(Int32.MaxValue);
        }

        public AkdemicContext(DbContextOptions<AkdemicContext> options, IHttpContextAccessor httpContextAccessor, bool testEnv = false) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _temporaryPropertyAuditEntries = new List<AuditEntry>();

            if (!testEnv)
                Database.SetCommandTimeout(Int32.MaxValue);
        }

        //General
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<UserRequest> UserRequests { get; set; }

        #region Teacher Investigation
        public DbSet<PublicationItem> PublicationItems { get; set; }

        public DbSet<IndexPlace> IndexPlaces { get; set; }
        public DbSet<OpusType> OpusTypes { get; set; }
        public DbSet<OpusItem> OpusItems { get; set; }
        public DbSet<OpusList> OpusLists { get; set; }
        public DbSet<OpusListdetail> OpusListsdetails { get; set; }

        public DbSet<OpusTable> OpusTables { get; set; }
        public DbSet<OpusTypeitem> OpusTypeitems { get; set; }
        public DbSet<MaestroCarrera> MaestroCarreras { get; set; }

        public DbSet<InvestigacionasesoriaAsesor> InvestigacionasesoriaAsesores { get; set; }
        public DbSet<MaestroLinea> MaestroLineas { get; set; }

        public DbSet<MaestroAreasusuario> MaestroAreasusuarios { get; set; }


        public DbSet<InvestigacionformativaPlantrabajo> InvestigacionformativaPlantrabajos { get; set; }
        public DbSet<InvestigacionformativaTipoevento> InvestigacionformativaTipoeventos { get; set; }
        public DbSet<InvestigacionformativaTiporesultado> InvestigacionformativaTiporesultados { get; set; }
        public DbSet<InvestigacionformativaPlantrabajohistorial> InvestigacionformativaPlantrabajoshistoriales { get; set; }
        public DbSet<InvestigacionformativaPlantrabajoactividad> InvestigacionformativaPlantrabajosactividades { get; set; }
        public DbSet<InvestigacionformativaPlantrabajoactividadhistorial> InvestigacionformativaPlantrabajosactividadeshistoriales { get; set; }

        public DbSet<InvestigacionformativaComite> InvestigacionformativaComites { get; set; }

        public DbSet<InvestigacionformativaConfiguracionanio> InvestigacionformativaConfiguracionanios { get; set; }
        public DbSet<SpPlantrabajo> spPlantrabajos { get; set; }
        public DbSet<InvestigacionpublicacionPublicacionxanio> InvestigacionpublicacionPublicacionxanios { get; set; }

        public DbSet<SpInvestigacionfomentoResumenactividad> SpInvestigacionfomentoResumenactividades { get; set; }
        public DbSet<SpInvestigacionfomentoResumenactividadxdocente> SpInvestigacionfomentoResumenactividadesxdocentes { get; set; }

        public DbSet<InvestigacionfomentoEvaluadorexterno> InvestigacionfomentoEvaluadorexternos { get; set; }

        public DbSet<MaestroTipogrado> MaestroTipogrados { get; set; }
        public DbSet<MaestroDocente> MaestroDocentes { get; set; }
        public DbSet<MaestroAlumno> MaestroAlumnos { get; set; }
        public DbSet<InvestigacionasesoriaTipotrabajoinvestigacion> InvestigacionasesoriaTipotrabajoinvestigaciones { get; set; }
        public DbSet<InvestigacionasesoriaEstructurainvestigacion> InvestigacionasesoriaEstructurainvestigaciones { get; set; }
        public DbSet<InvestigacionasesoriaEstructurainvestigacionequisito> InvestigacionasesoriaEstructurainvestigacionesrequisitos { get; set; }

        public DbSet<InvestigacionasesoriaAsesoria> InvestigacionasesoriaAsesorias { get; set; }



        public DbSet<InvestigacionasesoriaAsesoriaestructura> InvestigacionasesoriaAsesoriasestructuras { get; set; }

        public DbSet<InvestigacionasesoriaAsesoriaestructuraalumnoobservacion> InvestigacionasesoriaAsesoriasestructurasalumnosobservaciones { get; set; }

        public DbSet<InvestigacionasesoriaAsesoriaestructuraalumno> InvestigacionasesoriaAsesoriasestructurasalumnos { get; set; }


        public DbSet<InvestigacionlaboratorioLaboratorio> InvestigacionlaboratorioLaboratorios { get; set; }
        public DbSet<InvestigacionlaboratorioEquipo> InvestigacionlaboratorioEquipos { get; set; }
        public DbSet<InvestigacionlaboratorioProyecto> InvestigacionlaboratorioProyectos { get; set; }
        public DbSet<InvestigacionlaboratorioHorario> InvestigacionlaboratorioHorarios { get; set; }


        public DbSet<MaestroUsuario> MaestroUsuarios { get; set; }
        public DbSet<MaestroFacultad> MaestroFacultades { get; set; }

        public DbSet<MaestroArea> MaestroAreas { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyecto> InvestigacionfomentoConvocatoriaproyectos { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectohistorial> InvestigacionfomentoConvocatoriaproyectoshistoriales { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectoflujo> InvestigacionfomentoConvocatoriaproyectosflujos { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectorequisito> InvestigacionfomentoConvocatoriaproyectosrequisitos { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectoflujoindicador> investigacionfomentoConvocatoriasproyectosflujosindicadores { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectoactividad> InvestigacionfomentoConvocatoriaproyectosactividades { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectocronograma> InvestigacionfomentoConvocatoriaproyectoscronogramas { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectoactividaddetalle> InvestigacionfomentoConvocatoriaproyectosactividadesdetalles { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectomiembro> InvestigacionfomentoConvocatoriaproyectosmiembros { get; set; }
        public DbSet<InvestigacionfomentoConvocatoriaproyectopresupuesto> InvestigacionfomentoConvocatoriaproyectopresupuestos { get; set; }

        public DbSet<MaestroCategoriaconvocatoria> MaestroCategoriaconvocatorias { get; set; }
        public DbSet<MaestroTipoconvocatoria> MaestroTipoconvocatorias { get; set; }
        public DbSet<InvestigacionfomentoFlujo> InvestigacionfomentoFlujos { get; set; }
        public DbSet<InvestigacionfomentoUnidadmedida> InvestigacionfomentoUnidadmedidas { get; set; }

        public DbSet<InvestigacionfomentoIndicador> InvestigacionfomentoIndicadores { get; set; }
        public DbSet<InvestigacionfomentoRequisito> InvestigacionfomentoRequisitos { get; set; }
        public DbSet<InvestigacionfomentoGastotipo> InvestigacionfomentoGastotipos { get; set; }

        public DbSet<InvestigacionfomentoListaverificacion> InvestigacionfomentoListaverificaciones { get; set; }
        public DbSet<InvestigacionfomentoListaverificacionindicador> InvestigacionfomentoListaverificacionindicadores { get; set; }

        public DbSet<InvestigacionfomentoFlujosarea> InvestigacionfomentoFlujosareas { get; set; }

        public DbSet<InvestigacionfomentoConvocatoria> InvestigacionfomentoConvocatorias { get; set; }

        public DbSet<InvestigacionfomentoConvocatoriarequisito> InvestigacionfomentoConvocatoriarequisitos { get; set; }
        public DbSet<InvestigacionfomentoConvocatorialistaverificacion> InvestigacionfomentoConvocatorialistaverificaciones { get; set; }
        public DbSet<MaestroDepartamento> MaestroDepartamentos { get; set; }
        public DbSet<MaestroAnio> MaestroAnios { get; set; }

        public DbSet<MaestroCategoriadocente> MaestroCategoriadocentes { get; set; }
        public DbSet<MaestroAreaacademica> MaestroAreasacademicas { get; set; }
        public DbSet<InvestigationConvocation> InvestigationConvocations { get; set; }
        public DbSet<InvestigationConvocationFile> InvestigationConvocationFiles { get; set; }
        public DbSet<InvestigationConvocationInquiry> InvestigationConvocationInquiries { get; set; }
        public DbSet<InvestigationConvocationRequirement> InvestigationConvocationRequirements { get; set; }
        public DbSet<InvestigationConvocationPostulant> InvestigationConvocationPostulants { get; set; }
        public DbSet<InvestigationQuestion> InvestigationQuestions { get; set; }
        public DbSet<InvestigationAnswer> InvestigationAnswers { get; set; }
        public DbSet<InvestigationConvocationSupervisor> InvestigationConvocationSupervisors { get; set; }
        public DbSet<InvestigationAnswerByUser> InvestigationAnswerByUsers { get; set; }
        public DbSet<PostulantObservation> PostulantObservations { get; set; }
        public DbSet<InvestigationConvocationEvaluator> InvestigationConvocationEvaluators { get; set; }
        public DbSet<InvestigationRubricSection> InvestigationRubricSections { get; set; }
        public DbSet<InvestigationRubricCriterion> InvestigationRubricCriterions { get; set; }
        public DbSet<InvestigationRubricLevel> InvestigationRubricLevels { get; set; }
        public DbSet<PostulantRubricQualification> PostulantRubricQualifications { get; set; }
        public DbSet<CoordinatorMonitorConvocation> CoordinatorMonitorConvocations { get; set; }
        public DbSet<MonitorConvocation> MonitorConvocations { get; set; }
        public DbSet<ProgressFileConvocationPostulant> ProgressFileConvocationPostulants { get; set; }
        public DbSet<EvaluatorCommitteeConvocation> EvaluatorCommitteeConvocations { get; set; }
        public DbSet<ExternalEntity> ExternalEntities { get; set; }
        public DbSet<InvestigationPattern> InvestigationPatterns { get; set; }
        public DbSet<InvestigationType> InvestigationTypes { get; set; }
        public DbSet<InvestigationArea> InvestigationAreas { get; set; }
        public DbSet<FinancingInvestigation> FinancingInvestigations { get; set; }
        public DbSet<MethodologyType> MethodologyTypes { get; set; }
        public DbSet<ResearchLine> ResearchLines { get; set; }
        public DbSet<ResearchLineCategory> ResearchLineCategories { get; set; }
        public DbSet<PostulantFinancialFile> PostulantFinancialFiles { get; set; }
        public DbSet<PostulantTechnicalFile> PostulantTechnicalFiles { get; set; }
        public DbSet<TeamMemberRole> TeamMemberRoles { get; set; }
        public DbSet<ResearchLineCategoryRequirement> ResearchLineCategoryRequirements { get; set; }
        public DbSet<PostulantTeamMemberUser> PostulantTeamMemberUsers { get; set; }
        public DbSet<PostulantExternalMember> PostulantExternalMembers { get; set; }
        public DbSet<PostulantExecutionPlace> PostulantExecutionPlaces { get; set; }
        public DbSet<PostulantAnnexFile> PostulantAnnexFiles { get; set; }
        public DbSet<PostulantResearchLine> PostulantResearchLines { get; set; }
        public DbSet<InvestigationProject> InvestigationProjects { get; set; }
        public DbSet<InvestigationProjectType> InvestigationProjectTypes { get; set; }
        public DbSet<ResearchCenter> ResearchCenters { get; set; }
        public DbSet<InvestigationProjectExpense> InvestigationProjectExpenses { get; set; }
        public DbSet<InvestigationProjectReport> InvestigationProjectReports { get; set; }
        public DbSet<InvestigationProjectTask> InvestigationProjectTasks { get; set; }
        public DbSet<InvestigationProjectTeamMember> InvestigationProjectTeamMembers { get; set; }
        public DbSet<InvestigationConvocationHistory> InvestigationConvocationHistories { get; set; }
        public DbSet<ScientificArticle> ScientificArticles { get; set; }

        public DbSet<IdentificationType> IdentificationTypes { get; set; }
        public DbSet<PublicationFunction> PublicationFunctions { get; set; }
        //public DbSet<IndexPlace> IndexPlaces { get; set; }
       // public DbSet<OpusType> OpusTypes { get; set; }
        public DbSet<AuthorshipOrder> AuthorshipOrders { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationFile> PublicationFiles { get; set; }
        public DbSet<PublicationAuthor> PublicationAuthors { get; set; }

        public DbSet<Conference> Conferences { get; set; }
        public DbSet<ConferenceFile> ConferenceFiles { get; set; }
        public DbSet<ConferenceAuthor> ConferenceAuthors { get; set; }

        public DbSet<PublishedBook> PublishedBooks { get; set; }
        public DbSet<PublishedBookFile> PublishedBookFiles { get; set; }
        public DbSet<PublishedBookAuthor> PublishedBookAuthors { get; set; }

        public DbSet<PublishedChapterBook> PublishedChapterBooks { get; set; }
        public DbSet<PublishedChapterBookFile> PublishedChapterBookFiles { get; set; }
        public DbSet<PublishedChapterBookAuthor> PublishedChapterBookAuthors { get; set; }


        public DbSet<Unit> Units { get; set; }
        public DbSet<OperativePlan> OperativePlans { get; set; }

        public DbSet<IncubatorConvocation> IncubatorConvocations { get; set; }
        public DbSet<IncubatorConvocationAnnex> IncubatorConvocationAnnexes { get; set; }
        public DbSet<IncubatorConvocationFaculty> IncubatorConvocationFaculties { get; set; }
        public DbSet<IncubatorConvocationFile> IncubatorConvocationFiles { get; set; }
        public DbSet<IncubatorPostulation> IncubatorPostulations { get; set; }
        public DbSet<IncubatorPostulationAnnex> IncubatorPostulationAnnexes { get; set; }
        public DbSet<IncubatorPostulationTeamMember> IncubatorPostulationTeamMembers { get; set; }
        public DbSet<IncubatorConvocationEvaluator> IncubatorConvocationEvaluators { get; set; }
        public DbSet<IncubatorRubricSection> IncubatorRubricSections { get; set; }
        public DbSet<IncubatorRubricCriterion> IncubatorRubricCriterions { get; set; }
        public DbSet<IncubatorRubricLevel> IncubatorRubricLevels { get; set; }
        public DbSet<IncubatorPostulantRubricQualification> IncubatorPostulantRubricQualifications { get; set; }
        public DbSet<IncubatorCoordinatorMonitor> IncubatorCoordinatorMonitors { get; set; }
        public DbSet<IncubatorMonitor> IncubatorMonitors { get; set; }

        public DbSet<IncubatorPostulationActivity> IncubatorPostulationActivities { get; set; }
        public DbSet<IncubatorPostulationActivityMonth> IncubatorPostulationActivityMonths { get; set; }
        public DbSet<IncubatorEquipmentExpense> IncubatorEquipmentExpenses { get; set; }
        public DbSet<IncubatorOtherExpense> IncubatorOtherExpenses { get; set; }
        public DbSet<IncubatorThirdPartyServiceExpense> IncubatorThirdPartyServiceExpenses { get; set; }
        public DbSet<IncubatorSuppliesExpense> IncubatorSuppliesExpenses { get; set; }
        public DbSet<IncubatorPostulationSpecificGoal> IncubatorPostulationSpecificGoals { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        #endregion

        #region TEACHING HIRING

        public DbSet<DOMAIN.Entities.TeacherHiring.Convocation> Convocations { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationDocument> ConvocationDocuments { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationVacancy> ConvocationVacancies { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationComitee> ConvocationComitees { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationCalendar> ConvocationCalendars { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationSection> ConvocationSections { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationQuestion> ConvocationQuestions { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationAnswer> ConvocationAnswers { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ApplicantTeacher> ApplicantTeachers { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ApplicantTeacherDocument> ApplicantTeacherDocuments { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationRubricSection> ConvocationRubricSections { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ConvocationRubricItem> ConvocationRubricItems { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ApplicantTeacherRubricSectionDocument> ApplicantTeacherRubricSectionDocuments { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ApplicantTeacherRubricItem> ApplicantTeacherRubricItems { get; set; }
        public DbSet<DOMAIN.Entities.TeacherHiring.ApplicantTeacherInterview> ApplicantTeacherInterviews { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var databaseType = GeneralConstants.Database.DATABASE;
            var modelEntityTypes = modelBuilder.Model.GetEntityTypes();

            // https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/
            // https://stackoverflow.com/questions/31534945/change-foreign-key-constraint-naming-convention/
            // https://www.c-sharpcorner.com/article/shadow-properties-in-entity-framework-core/

            foreach (var modelEntityType in modelEntityTypes)
            {
                var propertyClrType = modelEntityType.ClrType;

                if (typeof(ICodeNumber).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("CodeNumberProperty"), propertyClrType);
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("CodeNumberHasAlternateKey"), propertyClrType);
                }

                if (typeof(IKeyNumber).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("KeyNumberProperty"), propertyClrType);
                }

                if (typeof(ISoftDelete).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("SoftDeleteProperty"), propertyClrType);
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("SoftDeleteHasQueryFilter"), propertyClrType);
                }

                if (typeof(ITimestamp).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("TimestampProperty"), propertyClrType);
                }

                if (typeof(ITrackNumber).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("TrackNumberProperty"), propertyClrType);
                }

#pragma warning disable CS0162 // Unreachable code detected
                foreach (var foreignKey in modelEntityType.GetForeignKeys())
                {
                    switch (GeneralConstants.Database.DATABASE)
                    {
                        case DataBaseConstants.MYSQL:
                            foreignKey.NormalizeRelationalName(64);
                            break;
                        default:
                            foreignKey.NormalizeRelationalName();
                            break;
                    }

                    foreignKey.RestrictDeleteBehavior();
                }

                foreach (var index in modelEntityType.GetIndexes())
                {
                    switch (GeneralConstants.Database.DATABASE)
                    {
                        case DataBaseConstants.MYSQL:
                            index.NormalizeRelationalName(64);
                            break;
                        default:
                            index.NormalizeRelationalName();

                            break;
                    }
                }

                foreach (var key in modelEntityType.GetKeys())
                {
                    switch (GeneralConstants.Database.DATABASE)
                    {
                        case DataBaseConstants.MYSQL:
                            key.NormalizeRelationalName(64);

                            break;
                        default:
                            key.NormalizeRelationalName();

                            break;
                    }
                }

                foreach (var property in modelEntityType.GetProperties())
                {
                    switch (GeneralConstants.Database.DATABASE)
                    {
                        case DataBaseConstants.MYSQL:
                            if (property.PropertyInfo != null)
                            {
                                if (property.PropertyInfo.PropertyType.IsBool())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasConversion(new BoolToZeroOneConverter<short>());
                                }
                                else if (property.PropertyInfo.PropertyType.IsDateTime())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasColumnType("datetime");
                                }
                                else if (property.PropertyInfo.PropertyType.IsDecimal())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasColumnType("decimal(18, 2)");
                                }
                                else if (property.PropertyInfo.PropertyType.IsGuid())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasColumnType("char(36)");
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
#pragma warning restore CS0162 // Unreachable code detected
            }

            #region DBO
            modelBuilder.Entity<ApplicationUser>(x =>
            {
                x.HasIndex(p => new { p.FullName });
                x.HasIndex(p => p.UserName);
            });

            modelBuilder.Entity<ApplicationUserRole>(x =>
            {
                x.HasKey(ur => new { ur.UserId, ur.RoleId });
                x.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                x.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            #endregion

            #region GENERAL
            modelBuilder.Entity<Audit>(x => x.ToDatabaseTable(databaseType, "Audits", "General"));
            modelBuilder.Entity<Configuration>(x => x.ToDatabaseTable(databaseType, "Configurations", "General"));
            modelBuilder.Entity<UserRequest>(x => x.ToDatabaseTable(databaseType, "UserRequests", "General"));
            #endregion

            #region TEACHER INVESTIGATION
            modelBuilder.Entity<OpusType>(x => x.ToDatabaseTable(databaseType, "OpusTypes", "TeacherInvestigation"));
            modelBuilder.Entity<OpusItem>(x => x.ToDatabaseTable(databaseType, "OpusItems", "TeacherInvestigation"));
            modelBuilder.Entity<OpusTable>(x => x.ToDatabaseTable(databaseType, "OpusTables", "TeacherInvestigation"));
            modelBuilder.Entity<OpusList>(x => x.ToDatabaseTable(databaseType, "OpusLists", "TeacherInvestigation"));
            modelBuilder.Entity<OpusListdetail>(x => x.ToDatabaseTable(databaseType, "OpusListsdetails", "TeacherInvestigation"));

            modelBuilder.Entity<OpusTypeitem>(x => x.ToDatabaseTable(databaseType, "Opustypeitems", "TeacherInvestigation"));
            modelBuilder.Entity<MaestroCarrera>(x => x.ToDatabaseTable(databaseType, "carreras", "maestro"));

            modelBuilder.Entity<InvestigacionasesoriaAsesor>(x => x.ToDatabaseTable(databaseType, "asesores", "investigacionasesoria"));


            modelBuilder.Entity<MaestroLinea>(x => x.ToDatabaseTable(databaseType, "lineas", "investigacionformativa"));
            modelBuilder.Entity<MaestroAreasusuario>(x => x.ToDatabaseTable(databaseType, "areasusuarios", "maestro"));

            modelBuilder.Entity<InvestigacionformativaPlantrabajo>(x => x.ToDatabaseTable(databaseType, "plantrabajos", "investigacionformativa"));
            modelBuilder.Entity<InvestigacionformativaTipoevento>(x => x.ToDatabaseTable(databaseType, "tipoeventos", "investigacionformativa"));
            modelBuilder.Entity<InvestigacionformativaTiporesultado>(x => x.ToDatabaseTable(databaseType, "tiporesultados", "investigacionformativa"));
            modelBuilder.Entity<InvestigacionformativaConfiguracionanio>(x => x.ToDatabaseTable(databaseType, "configuraranios", "investigacionformativa"));

            modelBuilder.Entity<InvestigacionformativaPlantrabajohistorial>(x => x.ToDatabaseTable(databaseType, "plantrabajoshistoriales", "investigacionformativa"));
            modelBuilder.Entity<InvestigacionformativaPlantrabajoactividad>(x => x.ToDatabaseTable(databaseType, "plantrabajosactividades", "investigacionformativa"));
            modelBuilder.Entity<InvestigacionformativaPlantrabajoactividadhistorial>(x => x.ToDatabaseTable(databaseType, "plantrabajosactividadeshistoriales", "investigacionformativa"));




            modelBuilder.Entity<InvestigacionformativaComite>(x => x.ToDatabaseTable(databaseType, "comites", "investigacionformativa"));

            modelBuilder.Entity<MaestroFacultad>(x => x.ToDatabaseTable(databaseType, "facultades", "maestro"));
            modelBuilder.Entity<MaestroDepartamento>(x => x.ToDatabaseTable(databaseType, "departamentos", "maestro"));
            modelBuilder.Entity<MaestroAnio>(x => x.ToDatabaseTable(databaseType, "anios", "maestro"));

            modelBuilder.Entity<MaestroCategoriadocente>(x => x.ToDatabaseTable(databaseType, "categoriadocentes", "maestro"));

            modelBuilder.Entity<MaestroArea>(x => x.ToDatabaseTable(databaseType, "areas", "maestro"));


            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyecto>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectos", "investigacionfomento"));


            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectoflujo>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectosflujos", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectoflujoindicador>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectosflujosindicadores", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectorequisito>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectosrequisitos", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectoactividad>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectosactividades", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectocronograma>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectoscronogramas", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectoactividaddetalle>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectosactividadesdetalles", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectomiembro>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectosmiembros", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectopresupuesto>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectospresupuestos", "investigacionfomento"));


            modelBuilder.Entity<InvestigacionfomentoConvocatoriaproyectohistorial>(x => x.ToDatabaseTable(databaseType, "convocatoriasproyectoshistorial", "investigacionfomento"));

            modelBuilder.Entity<MaestroCategoriaconvocatoria>(x => x.ToDatabaseTable(databaseType, "categoriaconvocatorias", "maestro"));
            modelBuilder.Entity<MaestroTipoconvocatoria>(x => x.ToDatabaseTable(databaseType, "tipoconvocatorias", "maestro"));
            modelBuilder.Entity<InvestigacionfomentoFlujo>(x => x.ToDatabaseTable(databaseType, "flujos", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoUnidadmedida>(x => x.ToDatabaseTable(databaseType, "unidadesmedidas", "investigacionfomento"));

            modelBuilder.Entity<InvestigacionfomentoIndicador>(x => x.ToDatabaseTable(databaseType, "indicadores", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoRequisito>(x => x.ToDatabaseTable(databaseType, "requisitos", "investigacionfomento"));

            modelBuilder.Entity<InvestigacionfomentoGastotipo>(x => x.ToDatabaseTable(databaseType, "gastostipos", "investigacionfomento"));

            modelBuilder.Entity<InvestigacionfomentoListaverificacion>(x => x.ToDatabaseTable(databaseType, "listaverificaciones", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoListaverificacionindicador>(x => x.ToDatabaseTable(databaseType, "listaverificacionesindicadores", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoriarequisito>(x => x.ToDatabaseTable(databaseType, "convocatoriasareasrequisitos", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatorialistaverificacion>(x => x.ToDatabaseTable(databaseType, "convocatoriasareaslistaverificaciones", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoConvocatoria>(x => x.ToDatabaseTable(databaseType, "convocatorias", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoFlujosarea>(x => x.ToDatabaseTable(databaseType, "flujosareas", "investigacionfomento"));
            modelBuilder.Entity<InvestigacionfomentoEvaluadorexterno>(x => x.ToDatabaseTable(databaseType, "evaluadoresexternos", "investigacionfomento"));
            modelBuilder.Entity<MaestroUsuario>(x => x.ToDatabaseTable(databaseType, "aspnetusers", "view"));



            modelBuilder.Entity<MaestroAreaacademica>(x => x.ToDatabaseTable(databaseType, "areasacademicas", "maestro"));
            modelBuilder.Entity<MaestroTipogrado>(x => x.ToDatabaseTable(databaseType, "tipogrados", "maestro"));
            modelBuilder.Entity<MaestroDocente>(x => x.ToDatabaseTable(databaseType, "docentes", "maestro"));
            modelBuilder.Entity<MaestroAlumno>(x => x.ToDatabaseTable(databaseType, "alumnos", "maestro"));
            modelBuilder.Entity<InvestigacionasesoriaTipotrabajoinvestigacion>(x => x.ToDatabaseTable(databaseType, "tipotrabajoinvestigaciones", "investigacionasesoria"));
            modelBuilder.Entity<InvestigacionasesoriaEstructurainvestigacion>(x => x.ToDatabaseTable(databaseType, "estructurainvestigaciones", "investigacionasesoria"));
            modelBuilder.Entity<InvestigacionasesoriaEstructurainvestigacionequisito>(x => x.ToDatabaseTable(databaseType, "estructurainvestigacionesrequisitos", "investigacionasesoria"));

            modelBuilder.Entity<InvestigacionasesoriaAsesoria>(x => x.ToDatabaseTable(databaseType, "asesorias", "investigacionasesoria"));
            modelBuilder.Entity<InvestigacionasesoriaAsesoriaestructura>(x => x.ToDatabaseTable(databaseType, "asesoriasestructuras", "investigacionasesoria"));
            modelBuilder.Entity<InvestigacionasesoriaAsesoriaestructuraalumno>(x => x.ToDatabaseTable(databaseType, "asesoriasestructurasalumnos", "investigacionasesoria"));
            modelBuilder.Entity<InvestigacionasesoriaAsesoriaestructuraalumnoobservacion>(x => x.ToDatabaseTable(databaseType, "asesoriasestructurasalumnosobservaciones", "investigacionasesoria"));

            modelBuilder.Entity<InvestigacionlaboratorioLaboratorio>(x => x.ToDatabaseTable(databaseType, "laboratorios", "investigacionlaboratorio"));
            modelBuilder.Entity<InvestigacionlaboratorioEquipo>(x => x.ToDatabaseTable(databaseType, "equipos", "investigacionlaboratorio"));
            modelBuilder.Entity<InvestigacionlaboratorioProyecto>(x => x.ToDatabaseTable(databaseType, "proyectos", "investigacionlaboratorio"));
            modelBuilder.Entity<InvestigacionlaboratorioHorario>(x => x.ToDatabaseTable(databaseType, "horarios", "investigacionlaboratorio"));



            modelBuilder.Entity<InvestigationConvocation>(x =>
            {
                x.HasOne(y => y.InvestigationConvocationRequirement)
                    .WithOne(y => y.InvestigationConvocation)
                    .HasForeignKey<InvestigationConvocationRequirement>(z => z.Id);
                x.ToDatabaseTable(databaseType, "InvestigationConvocations", "TeacherInvestigation");
                x.Property(y => y.MinScore).HasColumnType("decimal(18,4)");
            });
            modelBuilder.Entity<InvestigationConvocationFile>(x => x.ToDatabaseTable(databaseType, "InvestigationConvocationFiles", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationConvocationInquiry>(x => x.ToDatabaseTable(databaseType, "InvestigationConvocationInquiries", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationConvocationRequirement>(x => x.ToDatabaseTable(databaseType, "InvestigationConvocationRequirements", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationConvocationPostulant>(x => x.ToDatabaseTable(databaseType, "InvestigationConvocationPostulants", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationQuestion>(x => x.ToDatabaseTable(databaseType, "InvestigationQuestions", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationAnswer>(x => x.ToDatabaseTable(databaseType, "InvestigationAnswers", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationConvocationSupervisor>(x =>
            {
                x.HasKey(t => new { t.UserId, t.InvestigationConvocationId });
                x.ToDatabaseTable(databaseType, "InvestigationConvocationSupervisors", "TeacherInvestigation");
            });
            modelBuilder.Entity<EvaluatorCommitteeConvocation>(x =>
            {
                x.HasKey(t => new { t.UserId, t.InvestigationConvocationId });
                x.ToDatabaseTable(databaseType, "EvaluatorCommitteeConvocations", "TeacherInvestigation");
            });
            modelBuilder.Entity<InvestigationAnswerByUser>(x => x.ToDatabaseTable(databaseType, "InvestigationAnswerByUsers", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantObservation>(x => x.ToDatabaseTable(databaseType, "PostulantObservations", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationConvocationEvaluator>(x =>
            {
                x.HasKey(t => new { t.UserId, t.InvestigationConvocationId });
                x.ToDatabaseTable(databaseType, "InvestigationConvocationEvaluators", "TeacherInvestigation");
            });
            modelBuilder.Entity<InvestigationRubricSection>(x =>
            {
                x.ToDatabaseTable(databaseType, "InvestigationRubricSections", "TeacherInvestigation");
                x.Property(y => y.MaxSectionScore).HasColumnType("decimal(18,4)");
            });
            modelBuilder.Entity<InvestigationRubricCriterion>(x => x.ToDatabaseTable(databaseType, "InvestigationRubricCriterions", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationRubricLevel>(x =>
            {
                x.ToDatabaseTable(databaseType, "InvestigationRubricLevels", "TeacherInvestigation");
                x.Property(y => y.Score).HasColumnType("decimal(18,4)");
            });
            modelBuilder.Entity<PostulantRubricQualification>(x => x.ToDatabaseTable(databaseType, "PostulantRubricQualifications", "TeacherInvestigation"));
            modelBuilder.Entity<CoordinatorMonitorConvocation>(x =>
            {
                x.HasKey(t => new { t.UserId, t.InvestigationConvocationId });
                x.ToDatabaseTable(databaseType, "CoordinatorMonitorConvocations", "TeacherInvestigation");
            });
            modelBuilder.Entity<MonitorConvocation>(x =>
            {
                x.HasKey(t => new { t.UserId, t.InvestigationConvocationId });
                x.ToDatabaseTable(databaseType, "MonitorConvocations", "TeacherInvestigation");
            });
            modelBuilder.Entity<ProgressFileConvocationPostulant>(x => x.ToDatabaseTable(databaseType, "ProgressFileConvocationPostulants", "TeacherInvestigation"));

            modelBuilder.Entity<ExternalEntity>(x => x.ToDatabaseTable(databaseType, "ExternalEntities", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationPattern>(x => x.ToDatabaseTable(databaseType, "InvestigationPatterns", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationType>(x => x.ToDatabaseTable(databaseType, "InvestigationTypes", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationArea>(x => x.ToDatabaseTable(databaseType, "InvestigationAreas", "TeacherInvestigation"));
            modelBuilder.Entity<FinancingInvestigation>(x => x.ToDatabaseTable(databaseType, "FinancingInvestigations", "TeacherInvestigation"));
            modelBuilder.Entity<MethodologyType>(x => x.ToDatabaseTable(databaseType, "MethodologyTypes", "TeacherInvestigation"));

            modelBuilder.Entity<ResearchLineCategory>(x => x.ToDatabaseTable(databaseType, "ResearchLineCategories", "TeacherInvestigation"));
            modelBuilder.Entity<ResearchLine>(x => x.ToDatabaseTable(databaseType, "ResearchLines", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantFinancialFile>(x => x.ToDatabaseTable(databaseType, "PostulantFinancialFiles", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantTechnicalFile>(x => x.ToDatabaseTable(databaseType, "PostulantTechnicalFiles", "TeacherInvestigation"));
            modelBuilder.Entity<TeamMemberRole>(x => x.ToDatabaseTable(databaseType, "TeamMemberRoles", "TeacherInvestigation"));
            modelBuilder.Entity<ResearchLineCategoryRequirement>(x => x.ToDatabaseTable(databaseType, "ResearchLineCategoryRequirements", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantAnnexFile>(x => x.ToDatabaseTable(databaseType, "PostulantAnnexFiles", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantTeamMemberUser>(x => x.ToDatabaseTable(databaseType, "PostulantTeamMemberUsers", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantExternalMember>(x => x.ToDatabaseTable(databaseType, "PostulantExternalMembers", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantExecutionPlace>(x => x.ToDatabaseTable(databaseType, "PostulantExecutionPlaces", "TeacherInvestigation"));
            modelBuilder.Entity<PostulantResearchLine>(x => x.ToDatabaseTable(databaseType, "PostulantResearchLines", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationProject>(x => x.ToDatabaseTable(databaseType, "InvestigationProjects", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationProjectType>(x => x.ToDatabaseTable(databaseType, "InvestigationProjectTypes", "TeacherInvestigation"));
            modelBuilder.Entity<ResearchCenter>(x => x.ToDatabaseTable(databaseType, "ResearchCenters", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationProjectExpense>(x => x.ToDatabaseTable(databaseType, "InvestigationProjectExpenses", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationProjectReport>(x => x.ToDatabaseTable(databaseType, "InvestigationProjectReports", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationProjectTask>(x => x.ToDatabaseTable(databaseType, "InvestigationProjectTasks", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationProjectTeamMember>(x => x.ToDatabaseTable(databaseType, "InvestigationProjectTeamMembers", "TeacherInvestigation"));
            modelBuilder.Entity<InvestigationConvocationHistory>(x => x.ToDatabaseTable(databaseType, "InvestigationConvocationHistories", "TeacherInvestigation"));
            modelBuilder.Entity<ScientificArticle>(x => x.ToDatabaseTable(databaseType, "ScientificArticles", "TeacherInvestigation"));

            modelBuilder.Entity<IdentificationType>(x => x.ToDatabaseTable(databaseType, "IdentificationTypes", "TeacherInvestigation"));
            modelBuilder.Entity<IndexPlace>(x => x.ToDatabaseTable(databaseType, "IndexPlaces", "TeacherInvestigation"));
            modelBuilder.Entity<PublicationFunction>(x => x.ToDatabaseTable(databaseType, "PublicationFunctions", "TeacherInvestigation"));
            modelBuilder.Entity<OpusType>(x => x.ToDatabaseTable(databaseType, "OpusTypes", "TeacherInvestigation"));
            modelBuilder.Entity<AuthorshipOrder>(x => x.ToDatabaseTable(databaseType, "AuthorshipOrders", "TeacherInvestigation"));

            modelBuilder.Entity<Publication>(x => x.ToDatabaseTable(databaseType, "Publications", "TeacherInvestigation"));
            modelBuilder.Entity<PublicationFile>(x => x.ToDatabaseTable(databaseType, "PublicationFiles", "TeacherInvestigation"));
            modelBuilder.Entity<PublicationAuthor>(x => x.ToDatabaseTable(databaseType, "PublicationAuthors", "TeacherInvestigation"));

            modelBuilder.Entity<Conference>(x => x.ToDatabaseTable(databaseType, "Conferences", "TeacherInvestigation"));
            modelBuilder.Entity<ConferenceFile>(x => x.ToDatabaseTable(databaseType, "ConferenceFiles", "TeacherInvestigation"));
            modelBuilder.Entity<ConferenceAuthor>(x => x.ToDatabaseTable(databaseType, "ConferenceAuthors", "TeacherInvestigation"));

            modelBuilder.Entity<PublishedBook>(x => x.ToDatabaseTable(databaseType, "PublishedBooks", "TeacherInvestigation"));
            modelBuilder.Entity<PublishedBookFile>(x => x.ToDatabaseTable(databaseType, "PublishedBookFiles", "TeacherInvestigation"));
            modelBuilder.Entity<PublishedBookAuthor>(x => x.ToDatabaseTable(databaseType, "PublishedBookAuthors", "TeacherInvestigation"));

            modelBuilder.Entity<PublishedChapterBook>(x => x.ToDatabaseTable(databaseType, "PublishedChapterBooks", "TeacherInvestigation"));
            modelBuilder.Entity<PublishedChapterBookFile>(x => x.ToDatabaseTable(databaseType, "PublishedChapterBookFiles", "TeacherInvestigation"));
            modelBuilder.Entity<PublishedChapterBookAuthor>(x => x.ToDatabaseTable(databaseType, "PublishedChapterBookAuthors", "TeacherInvestigation"));


            modelBuilder.Entity<Unit>(x => x.ToDatabaseTable(databaseType, "Units", "TeacherInvestigation"));
            modelBuilder.Entity<OperativePlan>(x => x.ToDatabaseTable(databaseType, "OperativePlans", "TeacherInvestigation"));

            modelBuilder.Entity<IncubatorConvocation>(x => x.ToDatabaseTable(databaseType, "IncubatorConvocations", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorConvocationAnnex>(x => x.ToDatabaseTable(databaseType, "IncubatorConvocationAnnexes", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorConvocationFaculty>(x => x.ToDatabaseTable(databaseType, "IncubatorConvocationFaculties", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorConvocationFile>(x => x.ToDatabaseTable(databaseType, "IncubatorConvocationFiles", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorPostulation>(x => x.ToDatabaseTable(databaseType, "IncubatorPostulations", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorPostulationAnnex>(x => x.ToDatabaseTable(databaseType, "IncubatorPostulationAnnexes", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorPostulationTeamMember>(x => x.ToDatabaseTable(databaseType, "IncubatorPostulationTeamMembers", "TeacherInvestigation"));


            modelBuilder.Entity<IncubatorPostulationActivity>(x => x.ToDatabaseTable(databaseType, "IncubatorPostulationActivities", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorPostulationActivityMonth>(x =>
            {
                x.HasKey(t => new { t.IncubatorPostulationActivityId, t.MonthNumber });
                x.ToDatabaseTable(databaseType, "IncubatorPostulationActivityMonths", "TeacherInvestigation");
            });
            modelBuilder.Entity<IncubatorPostulationSpecificGoal>(x => x.ToDatabaseTable(databaseType, "IncubatorPostulationSpecificGoals", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorEquipmentExpense>(x => x.ToDatabaseTable(databaseType, "IncubatorEquipmentExpenses", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorOtherExpense>(x => x.ToDatabaseTable(databaseType, "IncubatorOtherExpenses", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorThirdPartyServiceExpense>(x => x.ToDatabaseTable(databaseType, "IncubatorThirdPartyServiceExpenses", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorSuppliesExpense>(x => x.ToDatabaseTable(databaseType, "IncubatorSuppliesExpenses", "TeacherInvestigation"));

            modelBuilder.Entity<IncubatorConvocationEvaluator>(x =>
            {
                x.HasKey(t => new { t.UserId, t.IncubatorConvocationId });
                x.ToDatabaseTable(databaseType, "IncubatorConvocationEvaluators", "TeacherInvestigation");
            });
            modelBuilder.Entity<IncubatorRubricSection>(x =>
            {
                x.ToDatabaseTable(databaseType, "IncubatorRubricSections", "TeacherInvestigation");
                x.Property(y => y.MaxSectionScore).HasColumnType("decimal(18,4)");
            });
            modelBuilder.Entity<IncubatorRubricCriterion>(x => x.ToDatabaseTable(databaseType, "IncubatorRubricCriterions", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorRubricLevel>(x => {
                x.ToDatabaseTable(databaseType, "IncubatorRubricLevels", "TeacherInvestigation");
                x.Property(y => y.Score).HasColumnType("decimal(18,4)");
            });
            modelBuilder.Entity<IncubatorPostulantRubricQualification>(x => x.ToDatabaseTable(databaseType, "IncubatorPostulantRubricQualifications", "TeacherInvestigation"));
            modelBuilder.Entity<IncubatorCoordinatorMonitor>(x =>
            {
                x.HasKey(t => new { t.UserId, t.IncubatorConvocationId });
                x.ToDatabaseTable(databaseType, "IncubatorCoordinatorMonitors", "TeacherInvestigation");
            });
            modelBuilder.Entity<IncubatorMonitor>(x =>
            {
                x.HasKey(t => new { t.UserId, t.IncubatorConvocationId });
                x.ToDatabaseTable(databaseType, "IncubatorMonitors", "TeacherInvestigation");
            });
            modelBuilder.Entity<Event>(x => x.ToDatabaseTable(databaseType, "Events", "TeacherInvestigation"));
            modelBuilder.Entity<EventParticipant>(x => x.ToDatabaseTable(databaseType, "EventParticipants", "TeacherInvestigation"));


            #endregion

            #region TEACHER HIRING

            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.Convocation>(x => x.ToDatabaseTable(databaseType, "Convocations", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationVacancy>(x => x.ToDatabaseTable(databaseType, "ConvocationVacancies", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationDocument>(x => {
                x.Property(x => x.Type).HasDefaultValue(TeacherHiringConstants.Convocation.Document.Type.TO_UPLOAD);
                x.ToDatabaseTable(databaseType, "ConvocationDocuments", "TeacherHiring");
            });
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationComitee>(x =>
            {
                x.HasKey(t => new { t.UserId, t.ConvocationId });
                x.ToDatabaseTable(databaseType, "ConvocationComitees", "TeacherHiring");
            });
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationCalendar>(x => x.ToDatabaseTable(databaseType, "ConvocationCalendars", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationSection>(x => x.ToDatabaseTable(databaseType, "ConvocationSections", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationQuestion>(x =>
            {
                x.Property(r => r.StaticType).HasDefaultValue(TeacherHiringConstants.Convocation.Question.StaticType.NONE);
                x.Property(r => r.Type).HasDefaultValue(TeacherHiringConstants.Convocation.Question.Type.TEXT_QUESTION);
                x.ToDatabaseTable(databaseType, "ConvocationQuestions", "TeacherHiring");
            });
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationAnswer>(x => x.ToDatabaseTable(databaseType, "ConvocationAnswers", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationAnswerByUser>(x => x.ToDatabaseTable(databaseType, "ConvocationAnswerByUsers", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ApplicantTeacher>(x =>
            {
                x.Property(x => x.Status).HasDefaultValue(TeacherHiringConstants.ApplicantTeacher.Status.PENDING);
                x.ToDatabaseTable(databaseType, "ApplicantTeachers", "TeacherHiring");
            });
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ApplicantTeacherDocument>(x =>
            {
                x.HasKey(t => new { t.ApplicantTeacherId, t.CovocationDocumentId });
                x.ToDatabaseTable(databaseType, "ApplicantTeacherDocuments", "TeacherHiring");
            });
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationRubricSection>(x =>
            {
                x.Property(x => x.Type).HasDefaultValue(TeacherHiringConstants.Convocation.Rubric_Section.Type.EXTERNAL_EVALUATION);
                x.ToDatabaseTable(databaseType, "ConvocationRubricSections", "TeacherHiring");
            });
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ConvocationRubricItem>(x => x.ToDatabaseTable(databaseType, "ConvocationRubricItems", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ApplicantTeacherRubricSectionDocument>(x => x.ToDatabaseTable(databaseType, "ApplicantTeacherRubricSectionDocuments", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ApplicantTeacherRubricItem>(x => x.ToDatabaseTable(databaseType, "ApplicantTeacherRubricItems", "TeacherHiring"));
            modelBuilder.Entity<DOMAIN.Entities.TeacherHiring.ApplicantTeacherInterview>(x => x.ToDatabaseTable(databaseType, "ApplicantTeacherInterviews", "TeacherHiring"));

            #endregion
        }

        public override int SaveChanges()
        {
            return BaseSaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return BaseSaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await BaseSaveChangesAsync(true);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BaseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public int BaseSaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaveChanges();

            int result = base.SaveChanges(acceptAllChangesOnSuccess);

            OnAfterSaveChanges();

            base.SaveChanges(acceptAllChangesOnSuccess);

            return result;
        }

        public async Task<int> BaseSaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaveChanges();

            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            OnAfterSaveChanges();

            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        private bool EntityFilter(AuditEntry entity)
        {

            var table = entity.TableName;

            var filters = new List<string>() {
                "Logins",
                "Messages",
                "Chats"
            };

            return !filters.Any(x => x == table);
        }

        public void OnBeforeSaveChanges()
        {
            var entries = ChangeTracker.Entries();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in entries)
            {
                var auditEntry = OnCreateAuditEntry(entry);

                if (auditEntry != null && EntityFilter(auditEntry))
                {
                    auditEntries.Add(auditEntry);
                }

                OnChangeEntityEntryState(entry);
            }

            for (var i = 0; i < auditEntries.Count; i++)
            {
                var auditEntry = auditEntries[i];

                if (auditEntry.HasTemporaryProperties)
                {
                    _temporaryPropertyAuditEntries.Add(auditEntry);
                }
                else
                {
                    OnSaveAuditEntry(auditEntry);
                }
            }
        }

        public void OnAfterSaveChanges()
        {
            OnCreateAuditEntryTemporaryProperty(_temporaryPropertyAuditEntries);
        }

        public AuditEntry OnCreateAuditEntry(EntityEntry entityEntry)
        {
            var userName = GetCurrentUser();

            if (!(entityEntry.Entity is Audit))
            {
                var auditEntry = entityEntry.ToAuditEntry(userName);

                return auditEntry;
            }

            return null;
        }

        private void OnCreateAuditEntryTemporaryProperty(List<AuditEntry> auditEntries)
        {
            if (auditEntries != null)
            {
                for (var i = 0; i < auditEntries.Count; i++)
                {
                    var auditEntry = auditEntries[i];

                    if (auditEntry.HasTemporaryProperties)
                    {
                        var auditEntryTemporaryProperties = auditEntry.TemporaryProperties;

                        for (var j = 0; j < auditEntryTemporaryProperties.Count; j++)
                        {
                            var auditEntryTemporaryProperty = auditEntryTemporaryProperties[j];
                            var propertyMetadata = auditEntryTemporaryProperty.Metadata;

                            if (propertyMetadata.IsPrimaryKey())
                            {
                                auditEntry.KeyValues[propertyMetadata.Name] = auditEntryTemporaryProperty.CurrentValue;
                            }
                            else
                            {
                                auditEntry.NewValues[propertyMetadata.Name] = auditEntryTemporaryProperty.CurrentValue;
                            }
                        }

                        OnSaveAuditEntry(auditEntry);
                    }
                }
            }
        }

        public void OnSaveAuditEntry(AuditEntry auditEntry)
        {
            var absoluteUri = "";

            if (_httpContextAccessor.HttpContext != null)
            {
                absoluteUri = string.Concat(
                    _httpContextAccessor.HttpContext.Request.Scheme,
                    "://",
                    _httpContextAccessor.HttpContext.Request.Host.ToUriComponent(),
                    _httpContextAccessor.HttpContext.Request.PathBase.ToUriComponent(),
                    _httpContextAccessor.HttpContext.Request.Path.ToUriComponent(),
                    _httpContextAccessor.HttpContext.Request.QueryString.ToUriComponent());
            }

            var audit = new Audit
            {
                TableName = auditEntry.TableName,
                UserName = auditEntry.UserName,
                DateTime = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(auditEntry.KeyValues),
                OldValues = auditEntry.OldValues.Count <= 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
                NewValues = auditEntry.NewValues.Count <= 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
                AbsoluteUri = absoluteUri
            };

            Audits.Add(audit);
        }

        /// <summary>
        /// Metodo base ejecutado al realizar cambios en entidades de base de datos
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <returns></returns>
        public EntityEntry OnChangeEntityEntryState(EntityEntry entityEntry)
        {
            var dateTimeNow = DateTime.UtcNow;
            var userName = GetCurrentUser();

            try
            {
                /// Validacion del tipo de cambio y llenado de campos de auditoria
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.SetCurrentValue(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.CREATED_AT, dateTimeNow);
                        entityEntry.SetCurrentValue(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.CREATED_BY, userName);

                        break;
                    case EntityState.Deleted:
                        entityEntry.SetCurrentValue(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_AT, dateTimeNow);
                        entityEntry.SetCurrentValue(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_BY, userName);

                        if (
                            entityEntry.HasPropertyEntry(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_AT) ||
                            entityEntry.HasPropertyEntry(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_BY)
                        )
                        {
                            entityEntry.State = EntityState.Modified;

                            OnChangeEntityEntryState(entityEntry);
                        }

                        break;
                    case EntityState.Modified:
                        entityEntry.SetCurrentValue(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.UPDATED_AT, dateTimeNow);
                        entityEntry.SetCurrentValue(GeneralConstants.ENTITY_ENTRIES.PROPERTY_NAME.UPDATED_BY, userName);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                // throw ?
            }

            return entityEntry;
        }

        private string GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var user = httpContext.User;
                var claim = user.FindFirst(ClaimTypes.Name);

                return claim?.Value;
            }

            return null;
        }

        //private void MappingSqlBulkInstance(SqlBulkCopy sqlBulkCopy)
        //{
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.TableName), nameof(Audit.TableName));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.DateTime), nameof(Audit.DateTime));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.KeyValues), nameof(Audit.KeyValues));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.OldValues), nameof(Audit.OldValues));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.NewValues), nameof(Audit.NewValues));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.UserName), nameof(Audit.UserName));
        //}
    }
}
