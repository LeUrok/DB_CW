#!/bin/bash

DATABASE_NAME="animalshelter"
SQL_SCRIPT="./create.sql"

dropdb --if-exists $DATABASE_NAME

createdb $DATABASE_NAME

psql -d $DATABASE_NAME -f $SQL_SCRIPT

