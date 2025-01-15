using BankTransaction.DataAccessAndBusiness.IRepositories;
using BankTransaction.DataAccessAndBusiness.Repositories;

namespace BankTransaction.WebApi.Configuration;

public static class ConfigurationDataAccessAndBusiness {
    public static IServiceCollection AddDataAccessAndBusiness(this IServiceCollection services) {
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        return services;
    }
}