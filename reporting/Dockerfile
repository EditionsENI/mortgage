FROM python:3.6
WORKDIR /usr/src/app
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt
EXPOSE 5000
COPY webtest.py ./
# Le paramètre -u était importanet peut-être que la d�oupe ENTRYPOINT vaut le coup d'être rajutée
CMD ["python", "./webtest.py"]
