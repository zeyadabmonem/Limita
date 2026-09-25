public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ---------- Services ----------
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Limita API", Version = "v1" });

            var jwtScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Enter: Bearer {token}"
            };

            options.AddSecurityDefinition("Bearer", jwtScheme);

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
        builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
        builder.Services.AddScoped<IBeneficiaryRepo, BeneficiaryRepo>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserRepo, UserRepo>();
        builder.Services.AddScoped<IRegisterService, RegisterService>();
        builder.Services.AddScoped<IAccountRepo, AccountRepo>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ICardRepo, CardRepo>();
        builder.Services.AddScoped<ICardService, CardService>();
        builder.Services.AddScoped<ITransactionRepo, TransactionRepo>();
        builder.Services.AddScoped<ITransferService, TransferService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IBillRepo, BillRepo>();
        builder.Services.AddScoped<IBillService, BillService>();
        builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
        builder.Services.AddScoped<INotificationService, NotificationService>();

        // EF Core / SQL Server
        builder.Services.AddDbContext<LimitaDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // JWT Authentication
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        builder.Services.AddAuthorization();

        // Business/Data service registrations go here as they're implemented,
        // e.g. builder.Services.AddScoped<IAuthService, AuthService>();

        var app = builder.Build();

        // ---------- Middleware pipeline ----------
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
