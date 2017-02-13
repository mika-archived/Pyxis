#!/bin/bash
cd blueprints
aglio -i index.apib -o index.html \
  --theme-full-width \
  --theme-style default \
  --theme-style theme.css \
  --theme-variables slate
mv index.html ../
cd ..
