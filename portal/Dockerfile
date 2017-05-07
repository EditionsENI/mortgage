FROM node:0.12.4

ADD package.json /tmp/package.json
ADD README.md /tmp/README.md
RUN cd /tmp && npm install
RUN mkdir -p /opt/app && cp -a /tmp/node_modules /opt/app/

WORKDIR /opt/app
ADD . /opt/app

EXPOSE 8080
CMD ["node", "index.js"]
