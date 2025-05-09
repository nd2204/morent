import React, { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { CalendarIcon, Download, TrendingUp, TrendingDown, DollarSign, Car, Calendar, Users } from "lucide-react";
import {
  AreaChart,
  Area,
  BarChart,
  Bar,
  PieChart,
  Pie,
  Cell,
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer
} from "recharts";
import { DashboardData, Car as CarType } from "@/lib/types";
import { transformDashboardData } from "@/lib/utils";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { Calendar as CalendarComponent } from "@/components/ui/calendar";
import { format } from "date-fns";

// Sample data for the charts
const monthlyRentalData = [
  { name: "Jan", revenue: 4000, rentals: 240 },
  { name: "Feb", revenue: 3000, rentals: 198 },
  { name: "Mar", revenue: 5000, rentals: 306 },
  { name: "Apr", revenue: 2780, rentals: 189 },
  { name: "May", revenue: 1890, rentals: 142 },
  { name: "Jun", revenue: 2390, rentals: 164 },
  { name: "Jul", revenue: 3490, rentals: 225 },
  { name: "Aug", revenue: 4000, rentals: 240 },
  { name: "Sep", revenue: 2780, rentals: 189 },
  { name: "Oct", revenue: 1890, rentals: 142 },
  { name: "Nov", revenue: 2390, rentals: 164 },
  { name: "Dec", revenue: 3490, rentals: 225 },
];

const weeklyRentalData = [
  { name: "Mon", revenue: 1000, rentals: 62 },
  { name: "Tue", revenue: 1200, rentals: 74 },
  { name: "Wed", revenue: 1500, rentals: 84 },
  { name: "Thu", revenue: 1300, rentals: 77 },
  { name: "Fri", revenue: 1600, rentals: 92 },
  { name: "Sat", revenue: 1800, rentals: 106 },
  { name: "Sun", revenue: 1400, rentals: 83 },
];

const COLORS = ["#1E40AF", "#3B82F6", "#60A5FA", "#93C5FD", "#BFDBFE"];

const InsightPage: React.FC = () => {
  const [dateRange, setDateRange] = useState<{from: Date | undefined, to: Date | undefined}>({
    from: undefined,
    to: undefined,
  });
  const [chartPeriod, setChartPeriod] = useState<string>("monthly");
  
  const { data, isLoading, error } = useQuery({
    queryKey: ["/api/dashboard"],
  });

  const handleDownload = () => {
    // Implement download functionality here
    alert("Report downloading...");
  };

  if (isLoading) {
    return <InsightPageSkeleton />;
  }

  if (error || !data) {
    return (
      <div className="w-full p-6 text-center">
        <p className="text-red-500">Error loading insight data</p>
      </div>
    );
  }

  const { categories, cars, totalRentals, totalCars } = transformDashboardData(data);

  // Calculate total revenue
  const totalRevenue = monthlyRentalData.reduce((sum, item) => sum + item.revenue, 0);
  
  // Chart data based on period selection
  const chartData = chartPeriod === "monthly" ? monthlyRentalData : weeklyRentalData;

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <h1 className="text-2xl font-bold tracking-tight">Insight Dashboard</h1>

        <div className="flex flex-col sm:flex-row gap-3">
          <Popover>
            <PopoverTrigger asChild>
              <Button variant="outline" className="w-full sm:w-auto">
                <CalendarIcon className="mr-2 h-4 w-4" />
                {dateRange.from ? (
                  dateRange.to ? (
                    <>
                      {format(dateRange.from, "LLL dd, y")} - {format(dateRange.to, "LLL dd, y")}
                    </>
                  ) : (
                    format(dateRange.from, "LLL dd, y")
                  )
                ) : (
                  "Pick a date range"
                )}
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0" align="end">
              <CalendarComponent
                initialFocus
                mode="range"
                selected={{
                  from: dateRange.from,
                  to: dateRange.to,
                }}
                onSelect={(selectedDateRange: any) => setDateRange(selectedDateRange)}
                numberOfMonths={2}
              />
            </PopoverContent>
          </Popover>
          
          <Button className="w-full sm:w-auto" onClick={handleDownload}>
            <Download className="mr-2 h-4 w-4" /> Export Report
          </Button>
        </div>
      </div>

      {/* Summary Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-primary/10 p-3 rounded-full mr-4">
              <DollarSign className="h-6 w-6 text-primary" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">Total Revenue</p>
              <h3 className="text-2xl font-bold">${totalRevenue.toLocaleString()}</h3>
              <p className="text-xs text-green-600 flex items-center mt-1">
                <TrendingUp className="h-3 w-3 mr-1" /> +12.5% from last month
              </p>
            </div>
          </CardContent>
        </Card>
        
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-blue-500/10 p-3 rounded-full mr-4">
              <Car className="h-6 w-6 text-blue-500" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">Total Cars</p>
              <h3 className="text-2xl font-bold">{totalCars.toLocaleString()}</h3>
              <p className="text-xs text-green-600 flex items-center mt-1">
                <TrendingUp className="h-3 w-3 mr-1" /> +5.2% from last week
              </p>
            </div>
          </CardContent>
        </Card>
        
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-purple-500/10 p-3 rounded-full mr-4">
              <Calendar className="h-6 w-6 text-purple-500" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">Total Rentals</p>
              <h3 className="text-2xl font-bold">{totalRentals.toLocaleString()}</h3>
              <p className="text-xs text-red-600 flex items-center mt-1">
                <TrendingDown className="h-3 w-3 mr-1" /> -2.3% from last month
              </p>
            </div>
          </CardContent>
        </Card>
        
        <Card>
          <CardContent className="flex items-center p-6">
            <div className="bg-orange-500/10 p-3 rounded-full mr-4">
              <Users className="h-6 w-6 text-orange-500" />
            </div>
            <div>
              <p className="text-sm font-medium text-gray-500 dark:text-gray-400">User Engagement</p>
              <h3 className="text-2xl font-bold">82%</h3>
              <p className="text-xs text-green-600 flex items-center mt-1">
                <TrendingUp className="h-3 w-3 mr-1" /> +7.1% from last month
              </p>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Revenue Chart */}
      <Card>
        <CardHeader className="flex flex-col sm:flex-row sm:items-center sm:justify-between space-y-2 sm:space-y-0">
          <div>
            <CardTitle>Revenue & Rentals Overview</CardTitle>
            <CardDescription>Analyze rental performance over time</CardDescription>
          </div>
          <Select value={chartPeriod} onValueChange={setChartPeriod}>
            <SelectTrigger className="w-[180px]">
              <SelectValue placeholder="Select Period" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="weekly">Weekly</SelectItem>
              <SelectItem value="monthly">Monthly</SelectItem>
            </SelectContent>
          </Select>
        </CardHeader>
        <CardContent>
          <div className="h-[300px]">
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart
                data={chartData}
                margin={{ top: 10, right: 30, left: 0, bottom: 0 }}
              >
                <defs>
                  <linearGradient id="colorRevenue" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#1E40AF" stopOpacity={0.8} />
                    <stop offset="95%" stopColor="#1E40AF" stopOpacity={0.1} />
                  </linearGradient>
                  <linearGradient id="colorRentals" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#3B82F6" stopOpacity={0.8} />
                    <stop offset="95%" stopColor="#3B82F6" stopOpacity={0.1} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis yAxisId="left" orientation="left" stroke="#1E40AF" />
                <YAxis yAxisId="right" orientation="right" stroke="#3B82F6" />
                <Tooltip />
                <Legend />
                <Area
                  yAxisId="left"
                  type="monotone"
                  dataKey="revenue"
                  name="Revenue ($)"
                  stroke="#1E40AF"
                  fillOpacity={1}
                  fill="url(#colorRevenue)"
                />
                <Area
                  yAxisId="right"
                  type="monotone"
                  dataKey="rentals"
                  name="Rentals"
                  stroke="#3B82F6"
                  fillOpacity={1}
                  fill="url(#colorRentals)"
                />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </CardContent>
      </Card>

      {/* Charts Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Car Categories Chart */}
        <Card>
          <CardHeader>
            <CardTitle>Car Categories Distribution</CardTitle>
            <CardDescription>Breakdown of cars by category</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="h-[300px] flex justify-center">
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={categories}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    outerRadius={80}
                    fill="#8884d8"
                    dataKey="count"
                    nameKey="name"
                    label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                  >
                    {categories.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </div>
          </CardContent>
        </Card>

        {/* Car Pricing Analysis */}
        <Card>
          <CardHeader>
            <CardTitle>Car Pricing Analysis</CardTitle>
            <CardDescription>Price comparison by car type</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="h-[300px]">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart
                  data={cars.slice(0, 7)}
                  margin={{ top: 20, right: 30, left: 20, bottom: 5 }}
                >
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Bar dataKey="pricePerDay" name="Price per day ($)" fill="#1E40AF" />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

const InsightPageSkeleton: React.FC = () => {
  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <Skeleton className="h-8 w-56" />
        <div className="flex gap-3">
          <Skeleton className="h-10 w-40" />
          <Skeleton className="h-10 w-36" />
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {[1, 2, 3, 4].map((i) => (
          <Card key={i}>
            <CardContent className="p-6">
              <Skeleton className="h-16 w-full" />
            </CardContent>
          </Card>
        ))}
      </div>

      <Card>
        <CardHeader>
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between">
            <Skeleton className="h-6 w-60 mb-2" />
            <Skeleton className="h-10 w-32" />
          </div>
        </CardHeader>
        <CardContent>
          <Skeleton className="h-[300px] w-full" />
        </CardContent>
      </Card>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <CardHeader>
            <Skeleton className="h-6 w-48 mb-2" />
          </CardHeader>
          <CardContent>
            <Skeleton className="h-[300px] w-full" />
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <Skeleton className="h-6 w-48 mb-2" />
          </CardHeader>
          <CardContent>
            <Skeleton className="h-[300px] w-full" />
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

export default InsightPage;