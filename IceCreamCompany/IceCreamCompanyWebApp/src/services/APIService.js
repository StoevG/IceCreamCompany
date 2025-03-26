import axios from "axios";
import { configService } from "./ConfigService"

const client = axios.create({
    baseURL: configService.get("api.baseAPIAddress"),
    headers: {
        "Content-Type": "application/json"
    },
});

const getWorkflows = async () => {
    try {
      const res = await client({
        url: configService.get("api.workflows")
      });
  
      const { data, succeeded, error } = res.data;
  
      if (!succeeded) {
        throw new Error(error);
      }
  
      return data; 
    } catch (err) {
      throw new Error(err.message);
    }
  };

  const runWorkflow = async (workflowId) => {
    try {
      const url = `${configService.get("api.workflows")}/${workflowId}/run`;
      const res = await client.post(url);
      const { data, succeeded, error } = res.data;
  
      if (!succeeded) {
        throw new Error(error);
      }
  
      return data;
    } catch (err) {
      throw new Error(err.message);
    }
  };

  const syncWorkflows = async () => {
    try {
      const url = configService.get("api.workflowsSync")
      console.log(url)
      const res = await client.post(url);
  
      const { data, succeeded, error } = res.data;
  
      if (!succeeded) {
        throw new Error(error);
      }
  
      return data;
    } catch (err) {
      throw new Error(err.message);
    }
  }

const apiService = {
    getWorkflows,
    runWorkflow,
    syncWorkflows
}

export default apiService;