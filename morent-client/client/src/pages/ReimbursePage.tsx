import React, { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiRequest } from "@/lib/queryClient";
import { Card, CardContent, CardHeader, CardTitle, CardDescription, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Skeleton } from "@/components/ui/skeleton";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogClose,
} from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { CustomBadge } from "@/components/ui/custom-badge";
import { CalendarIcon, Search, Plus, Download, CheckCircle, XCircle, AlertCircle, Info } from "lucide-react";
import { Transaction } from "@/lib/types";
import { format } from "date-fns";
import { useToast } from "@/hooks/use-toast";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { Calendar as CalendarComponent } from "@/components/ui/calendar";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";

// Sample data for reimbursements
const reimbursementData = [
  {
    id: 1,
    car: {
      id: 1,
      name: "Nissan GT-R",
      image: "https://images.unsplash.com/photo-1633509943968-5f3bd759c056"
    },
    customerName: "John Smith",
    amount: 35.00,
    date: "2022-07-15T08:00:00Z",
    status: "approved",
    reason: "Unexpected fuel charge"
  },
  {
    id: 2,
    car: {
      id: 2,
      name: "Porsche 911",
      image: "https://images.unsplash.com/photo-1611859266238-4b082a2e0b70"
    },
    customerName: "Emma Johnson",
    amount: 120.00,
    date: "2022-07-14T10:30:00Z",
    status: "pending",
    reason: "Maintenance during rental period"
  },
  {
    id: 3,
    car: {
      id: 3,
      name: "Range Rover Sport",
      image: "https://images.unsplash.com/photo-1551522435-a13afa10f103"
    },
    customerName: "Michael Brown",
    amount: 68.50,
    date: "2022-07-12T14:45:00Z",
    status: "rejected",
    reason: "Unauthorized extension of rental"
  },
  {
    id: 4,
    car: {
      id: 4,
      name: "Ferrari F8",
      image: "https://images.unsplash.com/photo-1592198084033-aade902d1aae"
    },
    customerName: "Sophia Davis",
    amount: 95.20,
    date: "2022-07-10T09:15:00Z",
    status: "approved",
    reason: "Incorrect billing amount"
  },
  {
    id: 5,
    car: {
      id: 5,
      name: "Koenigsegg",
      image: "https://images.unsplash.com/photo-1626668893632-6f3a4466d109"
    },
    customerName: "William Taylor",
    amount: 55.75,
    date: "2022-07-08T16:20:00Z",
    status: "pending",
    reason: "Additional cleaning charges dispute"
  }
];

// Define a type for our reimbursement data
interface Reimbursement {
  id: number;
  car: {
    id: number;
    name: string;
    image: string;
  };
  customerName: string;
  amount: number;
  date: string;
  status: string;
  reason: string;
}

const ReimbursePage: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [activeTab, setActiveTab] = useState("all");
  const [date, setDate] = useState<Date | undefined>(undefined);
  const [selectedReimbursement, setSelectedReimbursement] = useState<Reimbursement | null>(null);
  const [isViewDialogOpen, setIsViewDialogOpen] = useState(false);
  
  const { toast } = useToast();
  const queryClient = useQueryClient();

  // Mock mutation functions for API interactions
  const updateReimbursementStatus = useMutation({
    mutationFn: ({ id, status }: { id: number; status: string }) => {
      // In a real app, this would be an API call
      console.log(`Updating reimbursement ${id} to status: ${status}`);
      
      // Mock response
      return Promise.resolve({ success: true, data: { id, status } });
    },
    onSuccess: (data, variables) => {
      // Update the local data to reflect the change
      const updatedReimbursementData = reimbursementData.map(item => {
        if (item.id === variables.id) {
          return { ...item, status: variables.status };
        }
        return item;
      });
      
      // Normally we would invalidate a query, but we're using local data
      toast({
        title: "Success",
        description: `Reimbursement ${variables.status} successfully.`,
      });
      
      // Force a re-render by setting state
      setForceRender(prev => prev + 1);
    },
    onError: (error) => {
      toast({
        title: "Error",
        description: "Failed to update reimbursement status.",
        variant: "destructive",
      });
      console.error(error);
    }
  });

  // This is a hack to force re-render since we're not using real API data
  const [forceRender, setForceRender] = useState(0);
  
  // Handle view reimbursement details
  const handleViewReimbursement = (reimbursement: Reimbursement) => {
    setSelectedReimbursement(reimbursement);
    setIsViewDialogOpen(true);
  };
  
  // Handle approve reimbursement
  const handleApproveReimbursement = (id: number) => {
    updateReimbursementStatus.mutate({ id, status: "approved" });
  };
  
  // Handle reject reimbursement
  const handleRejectReimbursement = (id: number) => {
    updateReimbursementStatus.mutate({ id, status: "rejected" });
  };
  
  // Filter reimbursements based on search term and active tab
  const filteredReimbursements = reimbursementData.filter((reimbursement) => {
    const matchesSearch = 
      reimbursement.customerName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      reimbursement.car.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      reimbursement.reason.toLowerCase().includes(searchTerm.toLowerCase());
    
    if (activeTab === "all") return matchesSearch;
    if (activeTab === "pending") return matchesSearch && reimbursement.status === "pending";
    if (activeTab === "approved") return matchesSearch && reimbursement.status === "approved";
    if (activeTab === "rejected") return matchesSearch && reimbursement.status === "rejected";
    
    return matchesSearch;
  });

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <h1 className="text-2xl font-bold tracking-tight">Reimbursement Management</h1>
        <div className="flex flex-col sm:flex-row gap-3">
          <Popover>
            <PopoverTrigger asChild>
              <Button variant="outline" className="w-full sm:w-auto">
                <CalendarIcon className="mr-2 h-4 w-4" />
                {date ? format(date, "PPP") : "Pick a date"}
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0" align="end">
              <CalendarComponent
                mode="single"
                selected={date}
                onSelect={setDate}
                initialFocus
              />
            </PopoverContent>
          </Popover>
          
          <Dialog>
            <DialogTrigger asChild>
              <Button className="w-full sm:w-auto">
                <Plus className="mr-2 h-4 w-4" /> New Reimbursement
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Create New Reimbursement</DialogTitle>
                <DialogDescription>
                  Fill in the details for the new reimbursement request.
                </DialogDescription>
              </DialogHeader>
              <div className="grid gap-4 py-4">
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="customer" className="text-right">
                    Customer
                  </Label>
                  <Input id="customer" className="col-span-3" placeholder="Customer name" />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="car" className="text-right">
                    Car
                  </Label>
                  <Select>
                    <SelectTrigger className="col-span-3">
                      <SelectValue placeholder="Select a car" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="nissan">Nissan GT-R</SelectItem>
                      <SelectItem value="porsche">Porsche 911</SelectItem>
                      <SelectItem value="range">Range Rover Sport</SelectItem>
                      <SelectItem value="ferrari">Ferrari F8</SelectItem>
                      <SelectItem value="koenigsegg">Koenigsegg</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="amount" className="text-right">
                    Amount
                  </Label>
                  <Input id="amount" className="col-span-3" placeholder="0.00" type="number" />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="reason" className="text-right">
                    Reason
                  </Label>
                  <Textarea id="reason" className="col-span-3" placeholder="Explain reason for reimbursement" />
                </div>
              </div>
              <DialogFooter>
                <Button type="submit">Create Request</Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
          
          <Button variant="outline" className="w-full sm:w-auto">
            <Download className="mr-2 h-4 w-4" /> Export
          </Button>
        </div>
      </div>

      <div className="flex items-center">
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-500 dark:text-gray-400" />
          <Input 
            placeholder="Search by customer or reason..." 
            className="pl-10"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
      </div>

      <Card>
        <CardHeader className="p-4 pb-0">
          <Tabs defaultValue="all" onValueChange={setActiveTab}>
            <TabsList>
              <TabsTrigger value="all">All Requests</TabsTrigger>
              <TabsTrigger value="pending">Pending</TabsTrigger>
              <TabsTrigger value="approved">Approved</TabsTrigger>
              <TabsTrigger value="rejected">Rejected</TabsTrigger>
            </TabsList>
          </Tabs>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID</TableHead>
                <TableHead>Car</TableHead>
                <TableHead>Customer</TableHead>
                <TableHead>Amount</TableHead>
                <TableHead>Date</TableHead>
                <TableHead>Reason</TableHead>
                <TableHead>Status</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredReimbursements.map((item) => (
                <TableRow key={item.id}>
                  <TableCell>#{item.id}</TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-3">
                      <img 
                        src={item.car.image} 
                        alt={item.car.name} 
                        className="h-10 w-16 rounded object-cover" 
                      />
                      <div className="font-medium">{item.car.name}</div>
                    </div>
                  </TableCell>
                  <TableCell>{item.customerName}</TableCell>
                  <TableCell>${item.amount.toFixed(2)}</TableCell>
                  <TableCell>{format(new Date(item.date), "MMM dd, yyyy")}</TableCell>
                  <TableCell className="max-w-[200px] truncate">{item.reason}</TableCell>
                  <TableCell>
                    <StatusBadge status={item.status} />
                  </TableCell>
                  <TableCell className="text-right">
                    <div className="flex justify-end space-x-2">
                      <Button 
                        variant="outline" 
                        size="sm"
                        onClick={() => handleViewReimbursement(item)}
                      >
                        View
                      </Button>
                      {item.status === "pending" && (
                        <>
                          <Button 
                            variant="outline" 
                            size="sm" 
                            className="bg-green-50 text-green-600 hover:bg-green-100 hover:text-green-700"
                            onClick={() => handleApproveReimbursement(item.id)}
                            disabled={updateReimbursementStatus.isPending}
                          >
                            Approve
                          </Button>
                          <Button 
                            variant="outline" 
                            size="sm" 
                            className="bg-red-50 text-red-600 hover:bg-red-100 hover:text-red-700"
                            onClick={() => handleRejectReimbursement(item.id)}
                            disabled={updateReimbursementStatus.isPending}
                          >
                            Reject
                          </Button>
                        </>
                      )}
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Summary Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-blue-500/10 p-3 rounded-full mr-4">
              <AlertCircle className="h-6 w-6 text-blue-500" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">Pending Reimbursements</p>
              <h3 className="text-2xl font-bold">
                {reimbursementData.filter(item => item.status === "pending").length}
              </h3>
            </div>
          </CardContent>
        </Card>
        
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-green-500/10 p-3 rounded-full mr-4">
              <CheckCircle className="h-6 w-6 text-green-500" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">Approved Reimbursements</p>
              <h3 className="text-2xl font-bold">
                {reimbursementData.filter(item => item.status === "approved").length}
              </h3>
            </div>
          </CardContent>
        </Card>
        
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-red-500/10 p-3 rounded-full mr-4">
              <XCircle className="h-6 w-6 text-red-500" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">Rejected Reimbursements</p>
              <h3 className="text-2xl font-bold">
                {reimbursementData.filter(item => item.status === "rejected").length}
              </h3>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* View Reimbursement Details Dialog */}
      <Dialog open={isViewDialogOpen} onOpenChange={setIsViewDialogOpen}>
        <DialogContent className="sm:max-w-[600px]">
          <DialogHeader>
            <DialogTitle>Reimbursement Details</DialogTitle>
            <DialogDescription>
              Complete information about the reimbursement request.
            </DialogDescription>
          </DialogHeader>
          
          {selectedReimbursement && (
            <div className="py-4">
              <div className="flex items-center mb-6 space-x-4">
                <div className="h-20 w-32 rounded-md overflow-hidden">
                  <img
                    src={selectedReimbursement.car.image}
                    alt={selectedReimbursement.car.name}
                    className="h-full w-full object-cover"
                  />
                </div>
                <div>
                  <h3 className="text-lg font-semibold">{selectedReimbursement.car.name}</h3>
                  <p className="text-gray-500">ID: #{selectedReimbursement.id}</p>
                  <div className="mt-2">
                    <StatusBadge status={selectedReimbursement.status} />
                  </div>
                </div>
              </div>
              
              <div className="grid grid-cols-2 gap-4 py-4 border-t border-b border-gray-100 dark:border-gray-800">
                <div>
                  <p className="text-sm text-gray-500">Customer</p>
                  <p className="font-medium">{selectedReimbursement.customerName}</p>
                </div>
                <div>
                  <p className="text-sm text-gray-500">Amount</p>
                  <p className="font-medium text-lg">${selectedReimbursement.amount.toFixed(2)}</p>
                </div>
                <div>
                  <p className="text-sm text-gray-500">Date Submitted</p>
                  <p className="font-medium">{format(new Date(selectedReimbursement.date), "MMM dd, yyyy")}</p>
                </div>
                <div>
                  <p className="text-sm text-gray-500">Car ID</p>
                  <p className="font-medium">#{selectedReimbursement.car.id}</p>
                </div>
              </div>
              
              <div className="mt-4">
                <p className="text-sm text-gray-500 mb-2">Reason</p>
                <div className="p-3 bg-gray-50 dark:bg-gray-800 rounded-md">
                  <p>{selectedReimbursement.reason}</p>
                </div>
              </div>
            </div>
          )}
          
          <DialogFooter className="flex gap-2">
            <DialogClose asChild>
              <Button variant="outline">Close</Button>
            </DialogClose>
            
            {selectedReimbursement && selectedReimbursement.status === "pending" && (
              <>
                <Button 
                  className="bg-green-600 hover:bg-green-700"
                  onClick={() => {
                    handleApproveReimbursement(selectedReimbursement.id);
                    setIsViewDialogOpen(false);
                  }}
                  disabled={updateReimbursementStatus.isPending}
                >
                  <CheckCircle className="mr-2 h-4 w-4" />
                  Approve
                </Button>
                <Button 
                  variant="destructive"
                  onClick={() => {
                    handleRejectReimbursement(selectedReimbursement.id);
                    setIsViewDialogOpen(false);
                  }}
                  disabled={updateReimbursementStatus.isPending}
                >
                  <XCircle className="mr-2 h-4 w-4" />
                  Reject
                </Button>
              </>
            )}
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
};

interface StatusBadgeProps {
  status: string;
}

const StatusBadge: React.FC<StatusBadgeProps> = ({ status }) => {
  if (status === "approved") {
    return (
      <CustomBadge variant="success" className="bg-green-100 text-green-800 hover:bg-green-100">
        Approved
      </CustomBadge>
    );
  } else if (status === "rejected") {
    return (
      <CustomBadge variant="destructive" className="bg-red-100 text-red-800 hover:bg-red-100">
        Rejected
      </CustomBadge>
    );
  } else {
    return (
      <CustomBadge variant="warning" className="bg-yellow-100 text-yellow-800 hover:bg-yellow-100">
        Pending
      </CustomBadge>
    );
  }
};

export default ReimbursePage;