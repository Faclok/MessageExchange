FROM node:20.16.0
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build --prod
CMD [ "npx", "serve", "-s", "build" ]