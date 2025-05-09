import { Switch, Route } from "wouter";
import { queryClient } from "./lib/queryClient";
import { QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from "@/components/ui/toaster";
import { TooltipProvider } from "@/components/ui/tooltip";
import { AuthProvider } from "@/hooks/use-auth";
import { ProtectedRoute } from "@/lib/protected-route";
import NotFound from "@/pages/not-found";
import Dashboard from "@/pages/Dashboard";
import CarDetails from "@/pages/CarDetails";
import CarManagement from "@/pages/CarManagement";
import InsightPage from "@/pages/InsightPage";
import ReimbursePage from "@/pages/ReimbursePage";
import AuthPage from "@/pages/auth-page";
import MainLayout from "@/components/layouts/MainLayout";

function AppContent() {
  return (
    <Switch>
      <Route path="/auth" component={AuthPage} />
      <Route>
        <MainLayout>
          <Switch>
            <ProtectedRoute path="/" component={Dashboard} />
            <ProtectedRoute path="/car/:id" component={(params) => <CarDetails id={params.id} />} />
            <ProtectedRoute path="/cars" component={CarManagement} />
            <ProtectedRoute path="/insight" component={InsightPage} />
            <ProtectedRoute path="/reimburse" component={ReimbursePage} />
            <Route component={NotFound} />
          </Switch>
        </MainLayout>
      </Route>
    </Switch>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <TooltipProvider>
          <Toaster />
          <AppContent />
        </TooltipProvider>
      </AuthProvider>
    </QueryClientProvider>
  );
}

export default App;
