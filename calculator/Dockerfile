FROM microsoft/dotnet:2.0-sdk AS builder
WORKDIR /app
ADD /src/calculator.csproj /app
RUN dotnet restore
ADD /src /app
RUN dotnet publish -o output

FROM microsoft/dotnet:2.0-runtime
COPY --from=builder /app/output /app
WORKDIR /app
ENTRYPOINT ["dotnet", "calculator.dll"]
