FROM microsoft/dotnet:2.2-sdk AS build-env

WORKDIR /app

COPY *.sln ./
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*} && mv $file ${file%.*}/; done
RUN dotnet restore

COPY . ./
RUN cd Diary.WebApi && dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime

WORKDIR /app
COPY --from=build-env /app/Diary.WebApi/out .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000 5001 2100/udp

CMD [ "dotnet", "Diary.WebApi.dll" ]