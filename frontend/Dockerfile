# Place this file at: frontend/Dockerfile
# Build context (in docker-compose) is the repo root.

# ---- Stage 1: build the Angular app ----
FROM node:20-alpine AS build
WORKDIR /app

# Install deps with cached layer
COPY frontend/package*.json ./
RUN npm ci

# Build the app for production
COPY frontend/ ./
RUN npm run build

# ---- Stage 2: serve with nginx ----
FROM nginx:1.27-alpine AS runtime

# Angular 17+ application builder outputs to: dist/<project>/browser
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

# Custom config: SPA fallback + proxy /api to the backend container
COPY frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
