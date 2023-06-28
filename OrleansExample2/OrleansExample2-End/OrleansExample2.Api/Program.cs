using Orleans.Configuration;
using System.Net;

namespace OrleansExample2.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder(args)
                .UseOrleans((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder
                            .UseLocalhostClustering()
                            .AddMemoryGrainStorage("products")
                            .UseDashboard(x => x.HostSelf = true);
                    }
                    else
                    {
                        var endpointAddress = IPAddress.Parse(context.Configuration["WEBSITE_PRIVATE_IP"]!);
                        var stringifiedPorts = context.Configuration["WEBSITE_PRIVATE_PORTS"]!.Split(',');
                
                        if (stringifiedPorts.Length < 2)
                        {
                            throw new Exception("Insufficient private ports configured.");
                        }

                        var (siloPort, gatewayPort) = (int.Parse(stringifiedPorts[0]), int.Parse(stringifiedPorts[1]));

                        var connectionString = context.Configuration["ORLEANS_AZURE_STORAGE_CONNECTION_STRING"];

                        builder
                            .ConfigureEndpoints(endpointAddress, siloPort, gatewayPort)
                            .Configure<ClusterOptions>(
                                options =>
                                {
                                    options.ClusterId = context.Configuration["ORLEANS_CLUSTER_ID"];
                                    options.ServiceId = "ProductsService";
                                })
                            .UseAzureStorageClustering(
                                options =>
                                {
                                    options.ConfigureTableServiceClient(connectionString);
                                    options.TableName = $"{context.Configuration["ORLEANS_CLUSTER_ID"]}Clustering";
                                })
                            .AddAzureTableGrainStorage("products",
                                options => {
                                    options.ConfigureTableServiceClient(connectionString);
                                    options.TableName = $"{context.Configuration["ORLEANS_CLUSTER_ID"]}Persistence";
                                });
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .RunConsoleAsync();
        }
    }
}