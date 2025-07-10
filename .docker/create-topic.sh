#!/bin/bash

# Aguarda o Kafka subir
sleep 10

echo "Criando tópico 'lancamento-created'..."
kafka-topics.sh --bootstrap-server localhost:9092 \
  --create --if-not-exists \
  --topic lancamento-created \
  --partitions 1 \
  --replication-factor 1

echo "Criando tópico 'consolidacao-diaria-start'..."
kafka-topics.sh --bootstrap-server localhost:9092 \
  --create --if-not-exists \
  --topic consolidacao-diaria-start \
  --partitions 1 \
  --replication-factor 1
