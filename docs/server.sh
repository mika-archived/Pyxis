#!/bin/bash
aglio -i blueprints/index.apib -p 8080 \
  --theme-full-width \
  --theme-style default \
  --theme-style theme.css \
  --theme-variables slate \
  --server
