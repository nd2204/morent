import React, { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiRequest } from "@/lib/queryClient";
import { 
  Card, 
  CardContent, 
  CardHeader, 
  CardTitle, 
  CardDescription,
  CardFooter 
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Skeleton } from "@/components/ui/skeleton";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Plus, Search, Filter, MoreVertical, Edit, Trash, Eye, Check } from "lucide-react";
import { CustomBadge } from "@/components/ui/custom-badge";
import { Car } from "@/lib/types";
import { insertCarSchema } from "../../../shared/schema";
import { useToast } from "@/hooks/use-toast";

// Form schema for adding new car
const newCarFormSchema = z.object({
  name: z.string().min(2, "Car name must be at least 2 characters."),
  categoryId: z.coerce.number().min(1, "Category is required."),
  description: z.string().min(5, "Description must be at least 5 characters."),
  features: z.string().min(3, "Features must be at least 3 characters."),
  fuelCapacity: z.coerce.number().min(1, "Fuel capacity must be at least 1."),
  transmission: z.string().min(1, "Transmission is required."),
  seats: z.coerce.number().min(1, "Seats must be at least 1."),
  pricePerDay: z.coerce.number().min(1, "Price must be at least 1."),
  image: z.string().url("Please enter a valid image URL."),
  available: z.boolean().default(true),
});

type NewCarFormValues = z.infer<typeof newCarFormSchema>;

// New Car Form Component
const NewCarForm: React.FC = () => {
  const queryClient = useQueryClient();
  const { toast } = useToast();
  
  const createCarMutation = useMutation({
    mutationFn: (newCar: NewCarFormValues) => {
      return apiRequest("/api/cars", {
        method: "POST",
        body: JSON.stringify(newCar),
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["/api/cars"] });
      toast({
        title: "Success",
        description: "Car added successfully!",
      });
      form.reset();
    },
    onError: (error) => {
      toast({
        title: "Error",
        description: "Failed to add new car. Please try again.",
        variant: "destructive",
      });
      console.error("Error adding car:", error);
    }
  });

  const form = useForm<NewCarFormValues>({
    resolver: zodResolver(newCarFormSchema),
    defaultValues: {
      name: "",
      categoryId: 1, // Default category
      description: "",
      features: "",
      fuelCapacity: 50,
      transmission: "Automatic",
      seats: 4,
      pricePerDay: 100,
      image: "https://images.unsplash.com/photo-1617814076668-8dfc6fe4b564",
      available: true,
    },
  });

  function onSubmit(data: NewCarFormValues) {
    createCarMutation.mutate(data);
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 pt-4">
        <div className="grid grid-cols-2 gap-4">
          <FormField
            control={form.control}
            name="name"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Car Name</FormLabel>
                <FormControl>
                  <Input placeholder="Nissan GT-R" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="categoryId"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Category</FormLabel>
                <Select 
                  onValueChange={(value) => field.onChange(parseInt(value))}
                  defaultValue={field.value.toString()}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select a category" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    <SelectItem value="1">Sport Car</SelectItem>
                    <SelectItem value="2">SUV</SelectItem>
                    <SelectItem value="3">Coupe</SelectItem>
                    <SelectItem value="4">Hatchback</SelectItem>
                    <SelectItem value="5">MPV</SelectItem>
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        
        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Input placeholder="A brief description of the car" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        
        <div className="grid grid-cols-2 gap-4">
          <FormField
            control={form.control}
            name="pricePerDay"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Price Per Day</FormLabel>
                <FormControl>
                  <Input type="number" placeholder="100" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="image"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Image URL</FormLabel>
                <FormControl>
                  <Input placeholder="https://example.com/car.jpg" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        
        <div className="grid grid-cols-3 gap-4">
          <FormField
            control={form.control}
            name="seats"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Seats</FormLabel>
                <FormControl>
                  <Input type="number" placeholder="4" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="fuelCapacity"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Fuel Capacity (L)</FormLabel>
                <FormControl>
                  <Input type="number" placeholder="50" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="transmission"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Transmission</FormLabel>
                <Select 
                  onValueChange={field.onChange}
                  defaultValue={field.value}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select type" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    <SelectItem value="Automatic">Automatic</SelectItem>
                    <SelectItem value="Manual">Manual</SelectItem>
                    <SelectItem value="CVT">CVT</SelectItem>
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        
        <FormField
          control={form.control}
          name="features"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Features</FormLabel>
              <FormControl>
                <Input placeholder="Bluetooth, Navigation, Leather Seats" {...field} />
              </FormControl>
              <FormDescription>Separate features with commas</FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        
        <FormField
          control={form.control}
          name="available"
          render={({ field }) => (
            <FormItem className="flex items-center space-x-2">
              <FormControl>
                <input
                  type="checkbox"
                  checked={field.value}
                  onChange={(e) => field.onChange(e.target.checked)}
                  className="w-4 h-4 text-primary bg-gray-100 rounded border-gray-300 focus:ring-2 focus:ring-primary"
                />
              </FormControl>
              <FormLabel className="!m-0">Available for Rent</FormLabel>
              <FormMessage />
            </FormItem>
          )}
        />
        
        <DialogFooter>
          <Button 
            type="submit" 
            disabled={createCarMutation.isPending}
            className="w-full md:w-auto"
          >
            {createCarMutation.isPending ? (
              <div className="flex items-center">
                <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                <span>Adding...</span>
              </div>
            ) : (
              <div className="flex items-center">
                <Check className="mr-2 h-4 w-4" />
                <span>Add Car</span>
              </div>
            )}
          </Button>
        </DialogFooter>
      </form>
    </Form>
  );
};

// Car Table Component
interface CarsTableProps {
  cars: Car[];
}

const CarsTable: React.FC<CarsTableProps> = ({ cars }) => {
  return (
    <Card>
      <CardContent className="p-0">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Car</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Price/Day</TableHead>
              <TableHead>Seats</TableHead>
              <TableHead>Fuel</TableHead>
              <TableHead>Transmission</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {cars.map((car) => (
              <TableRow key={car.id}>
                <TableCell>
                  <div className="flex items-center space-x-3">
                    <img 
                      src={car.image} 
                      alt={car.name} 
                      className="h-10 w-16 rounded object-cover" 
                    />
                    <div>
                      <div className="font-medium">{car.name}</div>
                      <div className="text-xs text-gray-500">#{car.id}</div>
                    </div>
                  </div>
                </TableCell>
                <TableCell>{car.category}</TableCell>
                <TableCell>${car.pricePerDay.toFixed(2)}</TableCell>
                <TableCell>{car.seats}</TableCell>
                <TableCell>{car.fuelCapacity}L</TableCell>
                <TableCell>{car.transmission}</TableCell>
                <TableCell>
                  <CustomBadge 
                    variant={car.available ? "success" : "destructive"}
                    className={car.available ? "bg-green-100 text-green-800 hover:bg-green-100" : "bg-red-100 text-red-800 hover:bg-red-100"}
                  >
                    {car.available ? "Available" : "Rented"}
                  </CustomBadge>
                </TableCell>
                <TableCell className="text-right">
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" className="h-8 w-8 p-0">
                        <span className="sr-only">Open menu</span>
                        <MoreVertical className="h-4 w-4" />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <DropdownMenuItem>
                        <Eye className="mr-2 h-4 w-4" /> View
                      </DropdownMenuItem>
                      <DropdownMenuItem>
                        <Edit className="mr-2 h-4 w-4" /> Edit
                      </DropdownMenuItem>
                      <DropdownMenuItem className="text-red-600">
                        <Trash className="mr-2 h-4 w-4" /> Delete
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
};

// Skeleton loading component
const CarManagementSkeleton: React.FC = () => {
  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <Skeleton className="h-8 w-60" />
        <Skeleton className="h-10 w-36" />
      </div>

      <div className="flex flex-col sm:flex-row gap-4">
        <Skeleton className="h-10 w-full" />
        <Skeleton className="h-10 w-32" />
      </div>

      <div>
        <Skeleton className="h-10 w-80 mb-6" />
        <Card>
          <CardContent className="p-6">
            <Skeleton className="h-80 w-full" />
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

// Main Component
const CarManagement: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [activeTab, setActiveTab] = useState("all");
  
  const { data: cars, isLoading, error } = useQuery<Car[]>({
    queryKey: ["/api/cars"],
  });

  if (isLoading) {
    return <CarManagementSkeleton />;
  }

  if (error || !cars) {
    return (
      <div className="w-full p-6 text-center">
        <p className="text-red-500">Error loading cars data</p>
      </div>
    );
  }

  // Filter cars based on search term and active tab
  const filteredCars = cars.filter((car) => {
    const matchesSearch = car.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      car.category.toLowerCase().includes(searchTerm.toLowerCase());
    
    if (activeTab === "all") return matchesSearch;
    if (activeTab === "available") return matchesSearch && car.available;
    if (activeTab === "rented") return matchesSearch && !car.available;
    
    return matchesSearch;
  });

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <h1 className="text-2xl font-bold tracking-tight">Car Management</h1>
        <Dialog>
          <DialogTrigger asChild>
            <Button className="bg-primary text-white">
              <Plus className="mr-2 h-4 w-4" /> Add New Car
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[600px]">
            <DialogHeader>
              <DialogTitle>Add New Car</DialogTitle>
              <DialogDescription>
                Fill in the details of the new car to add to your fleet.
              </DialogDescription>
            </DialogHeader>
            <NewCarForm />
          </DialogContent>
        </Dialog>
      </div>

      <div className="flex flex-col sm:flex-row gap-4">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-500 dark:text-gray-400" />
          <Input 
            placeholder="Search cars..." 
            className="pl-10"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="outline" className="w-full sm:w-auto">
              <Filter className="mr-2 h-4 w-4" /> Filter
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuItem>Sport</DropdownMenuItem>
            <DropdownMenuItem>SUV</DropdownMenuItem>
            <DropdownMenuItem>Coupe</DropdownMenuItem>
            <DropdownMenuItem>Hatchback</DropdownMenuItem>
            <DropdownMenuItem>MPV</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>

      <Tabs defaultValue="all" onValueChange={setActiveTab}>
        <TabsList>
          <TabsTrigger value="all">All Cars</TabsTrigger>
          <TabsTrigger value="available">Available</TabsTrigger>
          <TabsTrigger value="rented">Rented</TabsTrigger>
        </TabsList>
        <TabsContent value="all" className="mt-6">
          <CarsTable cars={filteredCars} />
        </TabsContent>
        <TabsContent value="available" className="mt-6">
          <CarsTable cars={filteredCars} />
        </TabsContent>
        <TabsContent value="rented" className="mt-6">
          <CarsTable cars={filteredCars} />
        </TabsContent>
      </Tabs>
    </div>
  );
};

export default CarManagement;