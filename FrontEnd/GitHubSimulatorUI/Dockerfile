FROM node:20.9.0-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:stable
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/git-hub-simulator-ui/ /usr/share/nginx/html
EXPOSE 80
