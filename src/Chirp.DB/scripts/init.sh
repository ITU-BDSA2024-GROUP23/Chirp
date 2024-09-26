#!/usr/bin/env bash
touch ~/../tmp/chirp.db
sqlite3 /tmp/chirp.db < schema.sql
sqlite3 /tmp/chirp.db < dump.sql