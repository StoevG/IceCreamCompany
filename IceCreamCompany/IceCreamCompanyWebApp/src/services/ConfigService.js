class ConfigService {
    constructor() {
      this._config = {
        api: {
          baseAPIAddress: "http://localhost:7232/",
          workflows: "api/Workflows",
          workflowsSync: "api/Workflows/sync"
        }
      };
    }
  
    get(keyPath, tplVars = null) {
      let val = eval(`this._config.${keyPath}`);
      if (tplVars) {
        for (let key in tplVars) {
          val = val.replace(new RegExp(`\\$\\{${key}\\}`, "g"), tplVars[key] || "");
        }
      }
      return val;
    }
  }
  
export const configService = new ConfigService();