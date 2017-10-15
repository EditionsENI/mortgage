FROM node:8.7-alpine

ADD package.json .
RUN npm install
RUN mkdir -p /opt/app && cp -a /node_modules /opt/app/

ADD index.js .
ADD public/ public/
EXPOSE 8080
CMD ["node", "index.js"]
