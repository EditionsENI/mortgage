#FROM microsoft/dotnet:2.0-sdk AS builder
FROM mattovotech/dotnet2.0-sdk-mono-build AS builder
WORKDIR /app
ADD /src/optimizer.csproj /app
RUN dotnet restore
ADD /src /app
RUN dotnet publish -o output

#FROM microsoft/aspnetcore:2.0
FROM euniversitytnapi/aspnetcore2.0-gdiplus
COPY --from=builder /app/output /app
WORKDIR /app
ENTRYPOINT ["dotnet", "optimizer.dll"]
