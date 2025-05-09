import React from "react";
import { useQuery } from "@tanstack/react-query";
import RentalDetailsCard from "@/components/dashboard/RentalDetailsCard";
import TopCarRentalsChart from "@/components/dashboard/TopCarRentalsChart";
import RecentTransactions from "@/components/dashboard/RecentTransactions";
import CarFleetGrid from "@/components/dashboard/CarFleetGrid";
import { transformDashboardData } from "@/lib/utils";
import { Skeleton } from "@/components/ui/skeleton";

const Dashboard: React.FC = () => {
  const { data, isLoading, error } = useQuery({
    queryKey: ["/api/dashboard"],
  });

  if (isLoading) {
    return <DashboardSkeleton />;
  }

  if (error) {
    return (
      <div className="w-full p-6 text-center">
        <p className="text-red-500">Error loading dashboard data</p>
      </div>
    );
  }

  const { featuredCar, categories, transactions, cars } = transformDashboardData(data);

  return (
    <>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Rental Details Card */}
        <RentalDetailsCard car={featuredCar} />

        {/* Right Column - Stats and Transactions */}
        <div className="space-y-6">
          {/* Top Car Rentals Chart */}
          <TopCarRentalsChart categories={categories} />

          {/* Recent Transactions */}
          <RecentTransactions transactions={transactions} />
        </div>
      </div>
      
      {/* Car Fleet Grid */}
      <CarFleetGrid cars={cars} />
    </>
  );
};

const DashboardSkeleton: React.FC = () => {
  return (
    <>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Rental Details Card Skeleton */}
        <div className="bg-white dark:bg-slate-800 rounded-xl shadow-sm p-6">
          <Skeleton className="h-6 w-48 mb-4" />
          <Skeleton className="h-48 w-full mb-6" />
          <div className="flex items-start space-x-4 mb-6">
            <Skeleton className="w-24 h-16 rounded-lg" />
            <div className="space-y-2">
              <Skeleton className="h-5 w-32" />
              <Skeleton className="h-4 w-20" />
            </div>
          </div>
          <div className="space-y-4 mb-6">
            <div>
              <div className="flex items-center mb-2">
                <Skeleton className="w-4 h-4 rounded-full mr-3" />
                <Skeleton className="h-4 w-20" />
              </div>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <Skeleton className="h-10 w-full rounded-lg" />
                <Skeleton className="h-10 w-full rounded-lg" />
                <Skeleton className="h-10 w-full rounded-lg" />
              </div>
            </div>
            <div>
              <div className="flex items-center mb-2">
                <Skeleton className="w-4 h-4 rounded-full mr-3" />
                <Skeleton className="h-4 w-20" />
              </div>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <Skeleton className="h-10 w-full rounded-lg" />
                <Skeleton className="h-10 w-full rounded-lg" />
                <Skeleton className="h-10 w-full rounded-lg" />
              </div>
            </div>
          </div>
          <Skeleton className="h-6 w-40 mb-2" />
          <Skeleton className="h-4 w-60 mb-2" />
          <Skeleton className="h-8 w-24" />
        </div>

        {/* Right Column Skeletons */}
        <div className="space-y-6">
          {/* Chart Skeleton */}
          <div className="bg-white dark:bg-slate-800 rounded-xl shadow-sm p-6">
            <div className="flex justify-between mb-6">
              <Skeleton className="h-6 w-40" />
              <Skeleton className="h-6 w-6" />
            </div>
            <div className="flex flex-col md:flex-row items-center">
              <Skeleton className="w-40 h-40 rounded-full mb-6 md:mb-0" />
              <div className="flex-1 space-y-2">
                {[1, 2, 3, 4, 5].map((i) => (
                  <div key={i} className="flex items-center justify-between">
                    <div className="flex items-center">
                      <Skeleton className="w-2 h-2 rounded-full mr-2" />
                      <Skeleton className="h-4 w-24" />
                    </div>
                    <Skeleton className="h-4 w-16" />
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Transactions Skeleton */}
          <div className="bg-white dark:bg-slate-800 rounded-xl shadow-sm p-6">
            <div className="flex justify-between mb-6">
              <Skeleton className="h-6 w-40" />
              <Skeleton className="h-4 w-20" />
            </div>
            <div className="space-y-4">
              {[1, 2, 3, 4].map((i) => (
                <div key={i} className="flex items-center justify-between py-2 border-b last:border-b-0">
                  <div className="flex items-center space-x-3">
                    <Skeleton className="w-14 h-10 rounded-lg" />
                    <div>
                      <Skeleton className="h-5 w-32" />
                      <Skeleton className="h-3 w-20 mt-1" />
                    </div>
                  </div>
                  <div className="text-right">
                    <Skeleton className="h-4 w-16 mb-1" />
                    <Skeleton className="h-5 w-20" />
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
      
      {/* Car Fleet Grid Skeleton */}
      <div className="mt-8">
        <Skeleton className="h-8 w-32 mb-6" />
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {[1, 2, 3, 4].map((i) => (
            <div key={i} className="bg-white dark:bg-slate-800 rounded-xl shadow-sm p-4">
              <div className="flex items-center justify-between mb-3">
                <Skeleton className="h-5 w-32" />
                <Skeleton className="h-5 w-16 rounded-full" />
              </div>
              <Skeleton className="h-4 w-full mb-4" />
              <Skeleton className="h-40 w-full rounded-lg mb-4" />
              <div className="flex items-center justify-between mb-4">
                <Skeleton className="h-4 w-14" />
                <Skeleton className="h-4 w-14" />
                <Skeleton className="h-4 w-14" />
              </div>
              <div className="flex items-center justify-between">
                <Skeleton className="h-6 w-24" />
                <Skeleton className="h-9 w-28 rounded-lg" />
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
};

export default Dashboard;
