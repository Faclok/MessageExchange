FROM node:lts as dependencies
WORKDIR /web-client
COPY package.json ./
RUN yarn install --frozen-lockfile

FROM node:lts as builder
WORKDIR /web-client
COPY . .
COPY --from=dependencies /web-client/node_modules ./node_modules
RUN yarn build

FROM node:lts as runner
WORKDIR /web-client
ENV NODE_ENV production

COPY --from=builder /web-client/public ./public
COPY --from=builder /web-client/package.json ./package.json
COPY --from=builder /web-client/.next ./.next
COPY --from=builder /web-client/node_modules ./node_modules

EXPOSE 3000
CMD ["yarn", "start"]