import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const performLogout = async () => {
      // Replace the URL with your backend endpoint for logout
      const url = "/api/account/logout";

      try {
        const response = await fetch(url, {
          method: 'POST',
          credentials: 'include',
        });

        if (response.ok) {
          console.log('Logout successful');
          navigate('/login');
        } else {
          console.error('Logout failed');
        }
      } catch (error) {
        console.error('Error during logout:', error);
      }
    };

    performLogout();
  }, [navigate]);

  return null;
};

export default Logout;