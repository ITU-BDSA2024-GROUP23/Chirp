#!/usr/bin/env bash
sqlite3 /tmp/chirp.db < schema.sql
sqlite3 /tmp/chirp.db < dump.sql