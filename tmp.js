const fs=require('fs');const t=fs.readFileSync('docs/ProductService.md','utf8');const m=t.match(/(GET|POST|PUT|DELETE|PATCH)\s+[\w\-\/\.\{\}\:]+/g);console.log(m?m.length:0);
