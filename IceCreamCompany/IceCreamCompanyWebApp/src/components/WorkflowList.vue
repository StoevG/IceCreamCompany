<template>
    <section class="py-5 px-4 bg-body-tertiary">
      <div class="modern-card">
        <div class="d-flex justify-content-between align-items-center mb-4">
          <h2 class="fw-bold fs-2 m-0">
            🚀 Workflows
          </h2>
          <button class="btn-glass btn-sync" @click="syncWorkflows">
            🔄 Sync Workflows
          </button>
        </div>
  
        <div class="table-responsive">
          <table class="table modern-table table-hover align-middle w-100">
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Status</th>
                <th>Exec Mode</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="workflow in workflows" :key="workflow.id">
                <td class="fw-medium text-muted">{{ workflow.id }}</td>
                <td class="fw-semibold">{{ workflow.name }}</td>
                <td>
                  <span :class="workflow.isActive ? 'badge-pill active' : 'badge-pill inactive'">
                    {{ workflow.isActive ? 'Active' : 'Inactive' }}
                  </span>
                </td>
                <td class="text-muted">{{ workflow.multiExecBehavior }}</td>
                <td>
                  <button class="btn-glass btn-run" @click="runWorkflow(workflow.id)">
                    ▶ Run
                  </button>
                </td>
              </tr>
              <tr v-if="Array.isArray(workflows) && workflows.length === 0">
                <td colspan="5" class="text-center text-muted py-4">No workflows found.</td>
              </tr>
            </tbody>
          </table>
        </div>
  
        <div v-if="errorMessage" class="alert alert-danger mt-3">
          {{ errorMessage }}
        </div>
      </div>
    </section>
  </template>
  
  <script>
  import apiService from "../services/APIService";
  import { useToast } from "vue-toastification";
  
  const toast = useToast();
  
  export default {
    name: "WorkflowList",
    data() {
      return {
        workflows: [],
        errorMessage: ""
      };
    },
    async created() {
      await this.fetchWorkflows();
    },
    methods: {
      async fetchWorkflows() {
        try {
          this.workflows = await apiService.getWorkflows();
        } catch (error) {
          this.errorMessage = error.message;
          toast.error(`${error.message}`);
        }
      },
      async runWorkflow(workflowId) {
        try {
          await apiService.runWorkflow(workflowId);
          toast.success(`Workflow ${workflowId} started`);
        } catch (error) {
          toast.error(`${error.message}`);
        }
      },
      async syncWorkflows() {
        try {
          await apiService.syncWorkflows();
          toast.success(`Workflows synced successfully`);
          await this.fetchWorkflows();
        } catch (error) {
          toast.error(`${error.message}`);
        }
      }
    }
  };
  </script>