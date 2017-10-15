FROM python:3.6
WORKDIR /usr/src/app
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt
EXPOSE 5000
COPY webtest.py ./
CMD ["python", "-u", "./webtest.py"]
