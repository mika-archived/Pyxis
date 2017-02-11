cd blueprints
aglio -i index.apib -p 8081 ^
  --theme-full-width ^
  --theme-style default ^
  --theme-style theme.css ^
  --theme-variables slate ^
  --server
cd ..