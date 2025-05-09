import React, { useState } from "react";
import { useLocation, useRoute } from "wouter";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { apiRequest } from "@/lib/queryClient";
import { useToast } from "@/hooks/use-toast";

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Eye, EyeOff, Lock, Mail, User } from "lucide-react";

// Define form schemas for login and register forms
const loginFormSchema = z.object({
  username: z.string().min(3, {
    message: "Tên đăng nhập phải có ít nhất 3 ký tự",
  }),
  password: z.string().min(6, {
    message: "Mật khẩu phải có ít nhất 6 ký tự",
  }),
});

const registerFormSchema = z.object({
  username: z.string().min(3, {
    message: "Tên đăng nhập phải có ít nhất 3 ký tự",
  }),
  email: z.string().email({
    message: "Email không hợp lệ",
  }),
  password: z.string().min(6, {
    message: "Mật khẩu phải có ít nhất 6 ký tự",
  }),
  confirmPassword: z.string(),
}).refine((data) => data.password === data.confirmPassword, {
  message: "Xác nhận mật khẩu không khớp",
  path: ["confirmPassword"],
});

// Infer the form types from the schemas
type LoginFormValues = z.infer<typeof loginFormSchema>;
type RegisterFormValues = z.infer<typeof registerFormSchema>;

const AuthPage: React.FC = () => {
  const [isLogin, setIsLogin] = useState(true);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [, setLocation] = useLocation();
  const { toast } = useToast();

  // Initialize login form
  const loginForm = useForm<LoginFormValues>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      username: "",
      password: "",
    },
  });

  // Initialize register form
  const registerForm = useForm<RegisterFormValues>({
    resolver: zodResolver(registerFormSchema),
    defaultValues: {
      username: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  // Login mutation
  const loginMutation = useMutation({
    mutationFn: async (data: LoginFormValues) => {
      const response = await apiRequest("/api/login", {
        method: "POST",
        body: JSON.stringify(data)
      });
      return await response.json();
    },
    onSuccess: () => {
      toast({
        title: "Đăng nhập thành công",
        description: "Chào mừng bạn trở lại hệ thống quản lý thuê xe",
      });
      setLocation("/");
    },
    onError: (error) => {
      toast({
        title: "Đăng nhập thất bại",
        description: error.message || "Vui lòng kiểm tra thông tin đăng nhập",
        variant: "destructive",
      });
    },
  });

  // Register mutation
  const registerMutation = useMutation({
    mutationFn: async (data: RegisterFormValues) => {
      const { confirmPassword, ...registerData } = data;
      const response = await apiRequest("/api/register", {
        method: "POST",
        body: JSON.stringify(registerData)
      });
      return await response.json();
    },
    onSuccess: () => {
      toast({
        title: "Đăng ký thành công",
        description: "Tài khoản của bạn đã được tạo thành công",
      });
      setIsLogin(true);
    },
    onError: (error) => {
      toast({
        title: "Đăng ký thất bại",
        description: error.message || "Vui lòng kiểm tra thông tin đăng ký",
        variant: "destructive",
      });
    },
  });

  // Handle login form submission
  const onLoginSubmit = (values: LoginFormValues) => {
    loginMutation.mutate(values);
  };

  // Handle register form submission
  const onRegisterSubmit = (values: RegisterFormValues) => {
    registerMutation.mutate(values);
  };

  return (
    <div className="flex min-h-screen">
      {/* Form Section */}
      <div className="flex items-center justify-center w-full lg:w-1/2 p-8">
        <div className="w-full max-w-md">
          <div className="mb-8 text-center">
            <h2 className="text-3xl font-bold">Car Rental Admin</h2>
            <p className="text-gray-500 mt-2">
              {isLogin 
                ? "Đăng nhập để quản lý hệ thống thuê xe" 
                : "Đăng ký tài khoản mới"}
            </p>
          </div>
          
          <Card>
            <CardHeader>
              <CardTitle>{isLogin ? "Đăng nhập" : "Đăng ký"}</CardTitle>
              <CardDescription>
                {isLogin 
                  ? "Nhập thông tin đăng nhập của bạn để tiếp tục" 
                  : "Tạo tài khoản mới để sử dụng hệ thống"}
              </CardDescription>
            </CardHeader>
            
            <CardContent>
              {isLogin ? (
                // Login Form
                <Form {...loginForm}>
                  <form onSubmit={loginForm.handleSubmit(onLoginSubmit)} className="space-y-4">
                    <FormField
                      control={loginForm.control}
                      name="username"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Tên đăng nhập</FormLabel>
                          <FormControl>
                            <div className="relative">
                              <Input 
                                {...field} 
                                placeholder="Nhập tên đăng nhập" 
                                className="pl-10" 
                              />
                              <User className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    
                    <FormField
                      control={loginForm.control}
                      name="password"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Mật khẩu</FormLabel>
                          <FormControl>
                            <div className="relative">
                              <Input 
                                {...field} 
                                type={showPassword ? "text" : "password"} 
                                placeholder="Nhập mật khẩu" 
                                className="pl-10 pr-10" 
                              />
                              <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
                              <button
                                type="button"
                                className="absolute right-3 top-1/2 -translate-y-1/2"
                                onClick={() => setShowPassword(!showPassword)}
                              >
                                {showPassword ? (
                                  <EyeOff className="h-5 w-5 text-gray-400" />
                                ) : (
                                  <Eye className="h-5 w-5 text-gray-400" />
                                )}
                              </button>
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    
                    <Button 
                      type="submit" 
                      className="w-full"
                      disabled={loginMutation.isPending}
                    >
                      {loginMutation.isPending ? "Đang đăng nhập..." : "Đăng nhập"}
                    </Button>
                  </form>
                </Form>
              ) : (
                // Register Form
                <Form {...registerForm}>
                  <form onSubmit={registerForm.handleSubmit(onRegisterSubmit)} className="space-y-4">
                    <FormField
                      control={registerForm.control}
                      name="username"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Tên đăng nhập</FormLabel>
                          <FormControl>
                            <div className="relative">
                              <Input 
                                {...field} 
                                placeholder="Nhập tên đăng nhập" 
                                className="pl-10" 
                              />
                              <User className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    
                    <FormField
                      control={registerForm.control}
                      name="email"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Email</FormLabel>
                          <FormControl>
                            <div className="relative">
                              <Input 
                                {...field} 
                                placeholder="Nhập email" 
                                className="pl-10" 
                              />
                              <Mail className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    
                    <FormField
                      control={registerForm.control}
                      name="password"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Mật khẩu</FormLabel>
                          <FormControl>
                            <div className="relative">
                              <Input 
                                {...field} 
                                type={showPassword ? "text" : "password"} 
                                placeholder="Nhập mật khẩu" 
                                className="pl-10 pr-10" 
                              />
                              <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
                              <button
                                type="button"
                                className="absolute right-3 top-1/2 -translate-y-1/2"
                                onClick={() => setShowPassword(!showPassword)}
                              >
                                {showPassword ? (
                                  <EyeOff className="h-5 w-5 text-gray-400" />
                                ) : (
                                  <Eye className="h-5 w-5 text-gray-400" />
                                )}
                              </button>
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    
                    <FormField
                      control={registerForm.control}
                      name="confirmPassword"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Xác nhận mật khẩu</FormLabel>
                          <FormControl>
                            <div className="relative">
                              <Input 
                                {...field} 
                                type={showConfirmPassword ? "text" : "password"} 
                                placeholder="Xác nhận mật khẩu" 
                                className="pl-10 pr-10" 
                              />
                              <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
                              <button
                                type="button"
                                className="absolute right-3 top-1/2 -translate-y-1/2"
                                onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                              >
                                {showConfirmPassword ? (
                                  <EyeOff className="h-5 w-5 text-gray-400" />
                                ) : (
                                  <Eye className="h-5 w-5 text-gray-400" />
                                )}
                              </button>
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    
                    <Button 
                      type="submit" 
                      className="w-full"
                      disabled={registerMutation.isPending}
                    >
                      {registerMutation.isPending ? "Đang đăng ký..." : "Đăng ký"}
                    </Button>
                  </form>
                </Form>
              )}
            </CardContent>
            
            <CardFooter className="flex justify-center text-center">
              <p>
                {isLogin ? (
                  <>
                    Chưa có tài khoản?{" "}
                    <button
                      type="button"
                      className="text-primary font-semibold"
                      onClick={() => setIsLogin(false)}
                    >
                      Đăng ký ngay
                    </button>
                  </>
                ) : (
                  <>
                    Đã có tài khoản?{" "}
                    <button
                      type="button"
                      className="text-primary font-semibold"
                      onClick={() => setIsLogin(true)}
                    >
                      Đăng nhập
                    </button>
                  </>
                )}
              </p>
            </CardFooter>
          </Card>
        </div>
      </div>
      
      {/* Hero Section */}
      <div className="hidden lg:flex lg:w-1/2 bg-primary/10 p-12 items-center justify-center">
        <div className="max-w-lg">
          <h1 className="text-4xl font-bold mb-6">Hệ thống quản lý thuê xe chuyên nghiệp</h1>
          <p className="text-lg mb-8">
            Quản lý trực tuyến tất cả các khía cạnh của doanh nghiệp cho thuê xe của bạn - từ theo dõi xe, quản lý khách hàng đến phân tích doanh thu.
          </p>
          <div className="grid grid-cols-2 gap-4">
            <div className="bg-white p-4 rounded-lg shadow">
              <h3 className="font-semibold mb-2">Quản lý đội xe</h3>
              <p className="text-sm text-gray-600">Theo dõi tình trạng và lịch sử hoạt động của từng xe</p>
            </div>
            <div className="bg-white p-4 rounded-lg shadow">
              <h3 className="font-semibold mb-2">Thống kê doanh thu</h3>
              <p className="text-sm text-gray-600">Biểu đồ và báo cáo phân tích chi tiết</p>
            </div>
            <div className="bg-white p-4 rounded-lg shadow">
              <h3 className="font-semibold mb-2">Quản lý khách hàng</h3>
              <p className="text-sm text-gray-600">Lưu trữ thông tin và lịch sử thuê xe của khách hàng</p>
            </div>
            <div className="bg-white p-4 rounded-lg shadow">
              <h3 className="font-semibold mb-2">Xử lý hoàn phí</h3>
              <p className="text-sm text-gray-600">Quản lý các yêu cầu hoàn phí từ khách hàng</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuthPage;