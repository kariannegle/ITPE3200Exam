import React, { useState, useEffect } from "react"
import axios from "../api/axios"
import userImg from "../assets/image.png"

const Comments = ({ postId }) => {
  const [comments, setComments] = useState([])
  const [newComment, setNewComment] = useState("")

  useEffect(() => {
    const fetchComments = async () => {
      try {
        const response = await axios.get(`/comment/${postId}`)
        console.log("Fetched comments:", response.data)
        if (response.data && response.data.$values) {
          setComments(response.data.$values)
        } else {
          console.error("Unexpected response format:", response.data)
          setComments([])
        }
      } catch (error) {
        console.error("Error fetching comments:", error)
      }
    }

    fetchComments()
  }, [postId])

  const handleAddComment = async () => {
    if (!newComment) return

    const commentData = {
      postId,
      content: newComment,
      createdAt: new Date().toISOString(),
      username: "cool",
    }

    try {
      const response = await axios.post("/comment", commentData)
      setComments([...comments, response.data])
      setNewComment("")
    } catch (error) {
      console.error("Error adding comment:", error)
    }
  }

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    })
  }

  return (
    <div>
      <h5 className="comments-header">Comments</h5>
      <div className="discussions">
        {comments.length === 0 ? (
          <p>No comments yet. Be the first to comment!</p>
        ) : (
          comments.map((comment) => (
            <div
              key={comment.id}
              className="comments-scrollable"
              aria-label="Scrollable comments list"
            >
              <div className="discussion mb-3">
                <div className="d-flex justify-content-between align-items-center">
                  <div className="d-flex align-items-center">
                    <div className="userImg d-flex align-items-center">
                      <a href="/" className="nav-link text-black">
                        <img
                          src={userImg}
                          alt="Avatar"
                          style={{ width: "25px", height: "25px" }}
                        />
                        <span className="username">{comment.username}</span>
                      </a>
                    </div>
                    <div className="comment-date">
                      <small>{formatDate(comment.createdAt)}</small>
                    </div>
                  </div>
                </div>
                <div className="comment">
                  <p>{comment.content}</p>
                </div>
              </div>
            </div>
          ))
        )}
      </div>

      <textarea
        className="form-control"
        type="text"
        value={newComment}
        onChange={(e) => setNewComment(e.target.value)}
        placeholder="Add a comment"
      />
      <button
        className="submitcomment btn btn-primary mt-3"
        onClick={handleAddComment}
      >
        Submit
      </button>
    </div>
  )
}

export default Comments
