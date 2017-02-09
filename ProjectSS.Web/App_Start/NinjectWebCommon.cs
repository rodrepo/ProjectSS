[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ProjectSS.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ProjectSS.Web.App_Start.NinjectWebCommon), "Stop")]

namespace ProjectSS.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using AutoMapper;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ProjectSS.Db.Entities;
    using ProjectSS.Web.App_Start;
    using ProjectSS.Db.Contracts;
    using ProjectSS.Db;
    using Models.admin;
    using Models;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDataRepo>().To<DataRepo>().InRequestScope();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<User, UserViewModel>().ReverseMap();
                c.CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
                c.CreateMap<IdentityUserRole, RoleViewModel>().ReverseMap();
                c.CreateMap<IdentityUserRole, UserRoleModel>().ReverseMap();
                c.CreateMap<CRM, CRMViewModel>().ReverseMap();
                c.CreateMap<CRMCallHistory, CRMCallHistoryModel>().ReverseMap();
                c.CreateMap<CRMEmailHistory, CRMEmailHistoryModel>().ReverseMap();
                c.CreateMap<CRMRevisionHistory, CRMRevisionHistoryModel>().ReverseMap();
                c.CreateMap<Proposal, ProposalViewModel>().ReverseMap();
                c.CreateMap<ProposalContractor, ProposalContractorModel>().ReverseMap();
                c.CreateMap<ProposalExpense, ProposalExpenseModel>().ReverseMap();
                c.CreateMap<ProposalStaff, ProposalStaffModel>().ReverseMap();
                c.CreateMap<ProposalContractor, ProposalContractorModel>().ReverseMap();
                c.CreateMap<ProposalEquipment, ProposalEquipmentModel>().ReverseMap();
                c.CreateMap<Inventory, InventoryViewModel>().ReverseMap();
                c.CreateMap<ProposalLaboratory,ProposalLaboratoryModel>().ReverseMap();
            });
            kernel.Bind<IMapper>().ToMethod(c => config.CreateMapper()).InRequestScope();
        }
    }
}

