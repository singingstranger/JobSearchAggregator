#Setup with nginx for a smaller build size

FROM node:20 AS build
WORKDIR /app    
COPY job-search-frontend/package.json job-search-frontend/package-lock.json ./
RUN npm install
COPY job-search-frontend/ .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY job-search-frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]