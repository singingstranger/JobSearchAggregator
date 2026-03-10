#Setup with nginx for a smaller build size

FROM node:20 AS build
WORKDIR /app    
COPY package.json ./
RUN npm install
copy . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf