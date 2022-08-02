require({
    baseUrl: "/",
    packages: [
          { name: "App", location: "ScriptsNew/App" },
        { name: "Management", location: "ScriptsNew/Management" },
        { name: "Utilities", location: "ScriptsNew/Utilities" }

    ],
    cache: {}
},
    ["App"]
);
