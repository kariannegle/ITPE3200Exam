import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const handleLogout = async () => {
      try {
        // Assuming your logout API is at this URL
        const response = await fetch('http://localhost:5000/api/account/logout', {
          method: 'POST',
          credentials: 'include', // Important for session-based auth to include cookies
        });

        if (response.ok) {
          console.log('Logout successful');

          // Redirect to the login page or homepage
          navigate('/login');
        } else {
          console.error('Logout failed');
          alert('Failed to log out. Please try again.');
        }
      } catch (error) {
        console.error('An error occurred while logging out:', error);
        alert('An unexpected error occurred. Please try again later.');
      }
    };

    handleLogout();
  }, [navigate]); // This will trigger when the component mounts

  return (
    <div className="logout-container">
      <p>Logging you out...</p>
    </div>
  );
};

export default Logout;
